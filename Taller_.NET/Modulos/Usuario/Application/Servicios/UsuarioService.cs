using FluentValidation;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;

namespace Taller_.NET.Modulos.Usuario.Application.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioActualProvider _usuarioActualProvider;
        private readonly IAuth0AdministracionProvider _auth0AdministracionProvider;
        private readonly IValidator<CrearUsuarioDto> _crearUsuarioValidator;
        private readonly IValidator<ModificarUsuarioDto> _modificarUsuarioValidator;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IUsuarioActualProvider usuarioActualProvider,
            IAuth0AdministracionProvider auth0AdministracionProvider,
            IValidator<CrearUsuarioDto> crearUsuarioValidator,
            IValidator<ModificarUsuarioDto> modificarUsuarioValidator,
            ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioActualProvider = usuarioActualProvider;
            _auth0AdministracionProvider = auth0AdministracionProvider;
            _crearUsuarioValidator = crearUsuarioValidator;
            _modificarUsuarioValidator = modificarUsuarioValidator;
            _logger = logger;
        }

        public async Task<Guid> CrearUsuarioAsync(CrearUsuarioDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(_crearUsuarioValidator, dto, cancellationToken);
            await VerificarPermisoAsync(PermisosUsuarios.Crear, cancellationToken);

            var correo = NormalizarCorreo(dto.Correo);

            if (await _usuarioRepository.ExisteUsuarioConCorreoAsync(correo, cancellationToken))
            {
                throw new CorreoUsuarioDuplicadoException(correo);
            }

            var roles = await ObtenerRolesExistentesAsync(dto.RolesIds, cancellationToken);
            var usuario = new Usuario(dto.Nombre, dto.Apellido, correo, dto.Telefono);

            // La cuenta se crea primero en Auth0 para no persistir un usuario local
            // que no pueda iniciar sesion. Si la asignacion de roles o la persistencia
            // local falla, se elimina la cuenta recien creada para no dejar un usuario
            // huerfano en Auth0.
            var auth0UserId = await _auth0AdministracionProvider.CrearUsuarioAsync(
                usuario.Correo,
                dto.Contrasena,
                usuario.Nombre,
                usuario.Apellido,
                cancellationToken);

            try
            {
                await _auth0AdministracionProvider.ReemplazarRolesAsync(
                    auth0UserId,
                    roles.Select(rol => rol.Id).ToList(),
                    cancellationToken);

                usuario.VincularConAuth0(auth0UserId);

                await _usuarioRepository.AgregarAsync(usuario, cancellationToken);
                await _usuarioRepository.GuardarCambiosAsync(cancellationToken);
            }
            catch (Exception excepcion)
            {
                await CompensarCreacionEnAuth0Async(auth0UserId, excepcion);

                throw;
            }

            return usuario.Id;
        }

        public async Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await VerificarPermisoAsync(PermisosUsuarios.Actualizar, cancellationToken);

            var usuario = await ObtenerUsuarioExistenteAsync(id, cancellationToken);
            var roles = await ObtenerRolesDelUsuarioAsync(usuario, cancellationToken);

            return new ModificarUsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                RolesIds = roles.Select(rol => rol.Id).ToList()
            };
        }

        public async Task ModificarUsuarioAsync(ModificarUsuarioDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(_modificarUsuarioValidator, dto, cancellationToken);
            await VerificarPermisoAsync(PermisosUsuarios.Actualizar, cancellationToken);

            var usuario = await ObtenerUsuarioExistenteAsync(dto.Id, cancellationToken);
            var correo = NormalizarCorreo(dto.Correo);

            var usuarioConMismoCorreo = await _usuarioRepository.ObtenerPorCorreoAsync(correo, cancellationToken);

            if (usuarioConMismoCorreo is not null && usuarioConMismoCorreo.Id != dto.Id)
            {
                throw new CorreoUsuarioDuplicadoException(correo);
            }

            var roles = await ObtenerRolesExistentesAsync(dto.RolesIds, cancellationToken);
            var datosAnteriores = new DatosDeUsuario(
                usuario.Nombre,
                usuario.Apellido,
                usuario.Correo,
                usuario.Telefono);

            var rolesAnteriores = await ObtenerIdsDeRolesDelUsuarioAsync(usuario, cancellationToken);

            usuario.ActualizarDatosPersonales(dto.Nombre, dto.Apellido, correo, dto.Telefono);

            if (string.IsNullOrWhiteSpace(usuario.Auth0UserId))
            {
                // Usuarios creados antes de la integracion: se mantienen solo en la base
                // local hasta que se los vincule con su cuenta de Auth0. Los roles se
                // administran exclusivamente en Auth0, por lo que no se persisten.
                await GuardarUsuarioAsync(usuario, cancellationToken);

                return;
            }

            try
            {
                await _auth0AdministracionProvider.ActualizarUsuarioAsync(
                    usuario.Auth0UserId,
                    usuario.Correo,
                    usuario.Nombre,
                    usuario.Apellido,
                    cancellationToken);

                await _auth0AdministracionProvider.ReemplazarRolesAsync(
                    usuario.Auth0UserId,
                    roles.Select(rol => rol.Id).ToList(),
                    cancellationToken);
            }
            catch (Exception excepcion)
            {
                RestaurarDatos(usuario, datosAnteriores);
                await CompensarActualizacionEnAuth0Async(usuario, datosAnteriores, rolesAnteriores, excepcion);

                throw;
            }

            try
            {
                await GuardarUsuarioAsync(usuario, cancellationToken);
            }
            catch (Exception excepcion)
            {
                RestaurarDatos(usuario, datosAnteriores);
                await CompensarActualizacionEnAuth0Async(usuario, datosAnteriores, rolesAnteriores, excepcion);

                throw;
            }
        }

        public async Task EliminarUsuarioAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await VerificarPermisoAsync(PermisosUsuarios.Eliminar, cancellationToken);

            var usuario = await ObtenerUsuarioExistenteAsync(id, cancellationToken);

            await VerificarQueNoSeElimineASiMismoAsync(usuario, cancellationToken);

            if (!string.IsNullOrWhiteSpace(usuario.Auth0UserId))
            {
                await VerificarQueNoSeaElUltimoAdministradorAsync(usuario, cancellationToken);

                // Primero se revoca el acceso en Auth0: si esa operacion falla, el usuario
                // local se conserva y la eliminacion se puede reintentar.
                await _auth0AdministracionProvider.EliminarUsuarioAsync(usuario.Auth0UserId, cancellationToken);
            }

            try
            {
                await _usuarioRepository.EliminarAsync(usuario, cancellationToken);
                await _usuarioRepository.GuardarCambiosAsync(cancellationToken);
            }
            catch (Exception excepcion)
            {
                _logger.LogError(
                    excepcion,
                    "Se elimino el usuario {Auth0UserId} en Auth0 pero no se pudo eliminar el registro local {UsuarioId}.",
                    usuario.Auth0UserId,
                    usuario.Id);

                throw;
            }
        }

        public async Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default)
        {
            await VerificarPermisoAsync(PermisosUsuarios.Leer, cancellationToken);

            return await _auth0AdministracionProvider.ObtenerRolesAsync(cancellationToken);
        }

        public async Task<List<UsuarioListadoDto>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default)
        {
            await VerificarPermisoAsync(PermisosUsuarios.Leer, cancellationToken);

            var usuarios = await _usuarioRepository.ObtenerListadoAsync(cancellationToken);

            foreach (var usuario in usuarios)
            {
                usuario.Roles = await ObtenerNombresDeRolesAsync(usuario, cancellationToken);
            }

            return usuarios;
        }

        private async Task<List<string>> ObtenerNombresDeRolesAsync(
            UsuarioListadoDto usuario,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(usuario.Auth0UserId))
            {
                return new List<string>();
            }

            try
            {
                var roles = await _auth0AdministracionProvider.ObtenerRolesDeUsuarioAsync(
                    usuario.Auth0UserId,
                    cancellationToken);

                return roles.Select(rol => rol.Nombre).ToList();
            }
            catch (Exception excepcion)
            {
                _logger.LogWarning(
                    excepcion,
                    "No se pudieron obtener los roles de Auth0 para el usuario {UsuarioId}.",
                    usuario.Id);

                return new List<string>();
            }
        }

        private async Task<List<RolDto>> ObtenerRolesDelUsuarioAsync(
            Usuario usuario,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(usuario.Auth0UserId))
            {
                return new List<RolDto>();
            }

            return await _auth0AdministracionProvider.ObtenerRolesDeUsuarioAsync(
                usuario.Auth0UserId,
                cancellationToken);
        }

        private async Task<List<string>> ObtenerIdsDeRolesDelUsuarioAsync(
            Usuario usuario,
            CancellationToken cancellationToken)
        {
            var roles = await ObtenerRolesDelUsuarioAsync(usuario, cancellationToken);

            return roles.Select(rol => rol.Id).ToList();
        }

        private async Task VerificarQueNoSeaElUltimoAdministradorAsync(
            Usuario usuario,
            CancellationToken cancellationToken)
        {
            var roles = await ObtenerRolesDelUsuarioAsync(usuario, cancellationToken);

            var esAdministrador = roles.Any(rol =>
                string.Equals(rol.Nombre, RolesAuth0.Administrador, StringComparison.OrdinalIgnoreCase));

            if (!esAdministrador)
            {
                return;
            }

            var cantidadAdministradores = await _auth0AdministracionProvider.ObtenerCantidadUsuariosConRolAsync(
                RolesAuth0.Administrador,
                cancellationToken);

            if (cantidadAdministradores <= 1)
            {
                throw new NoSePuedeEliminarUltimoAdministradorException(
                    "No se puede eliminar al unico usuario con el rol de Administrador.");
            }
        }

        private async Task GuardarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            await _usuarioRepository.ActualizarAsync(usuario, cancellationToken);
            await _usuarioRepository.GuardarCambiosAsync(cancellationToken);
        }

        private async Task VerificarQueNoSeElimineASiMismoAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            const string mensaje = "Un administrador no puede eliminar su propia cuenta mientras mantenga una sesion activa.";

            var auth0UserIdActual = await _usuarioActualProvider.ObtenerAuth0UserIdAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(auth0UserIdActual))
            {
                var usuarioActual = await _usuarioRepository.ObtenerPorAuth0UserIdAsync(auth0UserIdActual, cancellationToken);

                if (usuarioActual is not null && usuarioActual.Id == usuario.Id)
                {
                    throw new UsuarioNoPuedeEliminarseException(mensaje);
                }
            }

            var correoActual = await _usuarioActualProvider.ObtenerCorreoAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(correoActual) &&
                string.Equals(correoActual.Trim(), usuario.Correo, StringComparison.OrdinalIgnoreCase))
            {
                throw new UsuarioNoPuedeEliminarseException(mensaje);
            }
        }

        private async Task CompensarCreacionEnAuth0Async(string auth0UserId, Exception excepcion)
        {
            try
            {
                // Se usa CancellationToken.None porque la compensacion debe ejecutarse
                // aunque la peticion original se haya cancelado.
                await _auth0AdministracionProvider.EliminarUsuarioAsync(auth0UserId, CancellationToken.None);
            }
            catch (Exception excepcionCompensacion)
            {
                _logger.LogError(
                    excepcionCompensacion,
                    "No se pudo eliminar de Auth0 el usuario {Auth0UserId} luego de fallar el alta local ({ErrorOriginal}).",
                    auth0UserId,
                    excepcion.Message);
            }
        }

        private async Task CompensarActualizacionEnAuth0Async(
            Usuario usuario,
            DatosDeUsuario datosAnteriores,
            IReadOnlyCollection<string> rolesAnteriores,
            Exception excepcion)
        {
            if (string.IsNullOrWhiteSpace(usuario.Auth0UserId))
            {
                return;
            }

            try
            {
                await _auth0AdministracionProvider.ActualizarUsuarioAsync(
                    usuario.Auth0UserId,
                    datosAnteriores.Correo,
                    datosAnteriores.Nombre,
                    datosAnteriores.Apellido,
                    CancellationToken.None);

                await _auth0AdministracionProvider.ReemplazarRolesAsync(
                    usuario.Auth0UserId,
                    rolesAnteriores,
                    CancellationToken.None);
            }
            catch (Exception excepcionCompensacion)
            {
                _logger.LogError(
                    excepcionCompensacion,
                    "No se pudieron restaurar en Auth0 los datos del usuario {Auth0UserId} luego de fallar la modificacion local ({ErrorOriginal}).",
                    usuario.Auth0UserId,
                    excepcion.Message);
            }
        }

        private static void RestaurarDatos(Usuario usuario, DatosDeUsuario datos)
        {
            usuario.ActualizarDatosPersonales(datos.Nombre, datos.Apellido, datos.Correo, datos.Telefono);
        }

        private async Task ValidarDtoAsync<T>(IValidator<T> validador, T dto, CancellationToken cancellationToken)
        {
            var resultado = await validador.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }

        private async Task VerificarPermisoAsync(string permiso, CancellationToken cancellationToken)
        {
            if (!await _usuarioActualProvider.TienePermisoAsync(permiso, cancellationToken))
            {
                throw new UsuarioNoAutorizadoException(
                    $"No tiene el permiso '{permiso}' requerido para gestionar los usuarios del sistema.");
            }
        }

        private async Task<Usuario> ObtenerUsuarioExistenteAsync(Guid id, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id, cancellationToken);

            if (usuario is null)
            {
                throw new UsuarioNoEncontradoException(id);
            }

            return usuario;
        }

        private async Task<List<RolDto>> ObtenerRolesExistentesAsync(
            IReadOnlyCollection<string> rolesIds,
            CancellationToken cancellationToken)
        {
            var rolesDisponibles = await _auth0AdministracionProvider.ObtenerRolesAsync(cancellationToken);

            var roles = rolesDisponibles
                .Where(rol => rolesIds.Contains(rol.Id, StringComparer.Ordinal))
                .ToList();

            var idsEncontrados = roles.Select(rol => rol.Id).ToHashSet(StringComparer.Ordinal);
            var idsFaltantes = rolesIds.Where(rolId => !idsEncontrados.Contains(rolId)).ToList();

            if (idsFaltantes.Count > 0)
            {
                throw new RolNoEncontradoException(idsFaltantes);
            }

            return roles;
        }

        private static string NormalizarCorreo(string correo)
        {
            return correo.Trim().ToLowerInvariant();
        }

        private sealed record DatosDeUsuario(
            string Nombre,
            string Apellido,
            string Correo,
            string Telefono);
    }
}
