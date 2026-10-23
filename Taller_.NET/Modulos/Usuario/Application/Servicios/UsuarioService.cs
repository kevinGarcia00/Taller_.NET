using FluentValidation;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia.Configuraciones;

namespace Taller_.NET.Modulos.Usuario.Application.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioActualProvider _usuarioActualProvider;
        private readonly IValidator<CrearUsuarioDto> _crearUsuarioValidator;
        private readonly IValidator<ModificarUsuarioDto> _modificarUsuarioValidator;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IUsuarioActualProvider usuarioActualProvider,
            IValidator<CrearUsuarioDto> crearUsuarioValidator,
            IValidator<ModificarUsuarioDto> modificarUsuarioValidator)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioActualProvider = usuarioActualProvider;
            _crearUsuarioValidator = crearUsuarioValidator;
            _modificarUsuarioValidator = modificarUsuarioValidator;
        }

        public async Task<Guid> CrearUsuarioAsync(CrearUsuarioDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(_crearUsuarioValidator, dto, cancellationToken);
            await VerificarPermisoDeAdministradorAsync(cancellationToken);

            var correo = NormalizarCorreo(dto.Correo);

            if (await _usuarioRepository.ExisteUsuarioConCorreoAsync(correo, cancellationToken))
            {
                throw new CorreoUsuarioDuplicadoException(correo);
            }

            var roles = await ObtenerRolesExistentesAsync(dto.RolesIds, cancellationToken);

            var usuario = new Usuario(dto.Nombre, dto.Apellido, correo, dto.Telefono);

            foreach (var rol in roles)
            {
                usuario.AsignarRol(rol);
            }

            await _usuarioRepository.AgregarAsync(usuario, cancellationToken);

            return usuario.Id;
        }

        public async Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var usuario = await ObtenerUsuarioExistenteAsync(id, cancellationToken);

            return new ModificarUsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                RolesIds = usuario.Roles.Select(rol => rol.Id).ToList()
            };
        }

        public async Task ModificarUsuarioAsync(ModificarUsuarioDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(_modificarUsuarioValidator, dto, cancellationToken);
            await VerificarPermisoDeAdministradorAsync(cancellationToken);

            var usuario = await ObtenerUsuarioExistenteAsync(dto.Id, cancellationToken);
            var correo = NormalizarCorreo(dto.Correo);

            var usuarioConMismoCorreo = await _usuarioRepository.ObtenerPorCorreoAsync(correo, cancellationToken);

            if (usuarioConMismoCorreo is not null && usuarioConMismoCorreo.Id != dto.Id)
            {
                throw new CorreoUsuarioDuplicadoException(correo);
            }

            var roles = await ObtenerRolesExistentesAsync(dto.RolesIds, cancellationToken);

            usuario.ActualizarDatosPersonales(dto.Nombre, dto.Apellido, correo, dto.Telefono);
            usuario.ActualizarRoles(roles);

            await _usuarioRepository.ActualizarAsync(usuario, cancellationToken);
        }

        public async Task EliminarUsuarioAsync(Guid id, Guid idUsuarioAutenticado = default, CancellationToken cancellationToken = default)
        {
            await VerificarPermisoDeAdministradorAsync(cancellationToken);

            // 1. Validar que el usuario no intente eliminarse a sí mismo
            // (solo aplicamos esta validación si se proporcionó un ID de usuario autenticado)
            // Por ahora, en development sin Auth0, se omite esta validación para permitir el delete
            if (idUsuarioAutenticado != Guid.Empty && id == idUsuarioAutenticado)
            {
                // Validación omitida temporalmente hasta implementar Auth0 con login
                // Para development, se permite el delete sin validación de self-delete
            }

            // 2. Consultar el usuario por Id incluyendo sus roles
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id, cancellationToken);

            // 3. Verificar la existencia del usuario
            if (usuario is null)
            {
                throw new UsuarioNoEncontradoException(id);
            }

            // 4. Validar que no sea el único usuario con el rol "Administrador"
            var cantidadAdministradores = await _usuarioRepository.ObtenerCantidadAdministradoresAsync(cancellationToken);

            if (cantidadAdministradores <= 1 && usuario.TieneRol(Rol.NombreAdministrador))
            {
                throw new NoSePuedeEliminarUltimoAdministradorException("No se puede eliminar al único usuario con el rol de Administrador.");
            }

            // 5. Remover la entidad y persistir cambios
            await _usuarioRepository.Eliminar(usuario, cancellationToken);
        }

        public async Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _usuarioRepository.ObtenerTodosLosRolesAsync(cancellationToken);

            return roles.Select(rol => new RolDto { Id = rol.Id, Nombre = rol.Nombre }).ToList();
        }

        public async Task<List<UsuarioListadoDto>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default)
        {
            await VerificarPermisoDeAdministradorAsync(cancellationToken);

            return await _usuarioRepository.ObtenerListadoAsync(cancellationToken);
        }

        private async Task ValidarDtoAsync<T>(IValidator<T> validador, T dto, CancellationToken cancellationToken)
        {
            var resultado = await validador.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }

        private async Task VerificarPermisoDeAdministradorAsync(CancellationToken cancellationToken)
        {
            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException("Solo un administrador puede gestionar los usuarios del sistema.");
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

        private async Task<List<Rol>> ObtenerRolesExistentesAsync(IReadOnlyCollection<long> rolesIds, CancellationToken cancellationToken)
        {
            var roles = await _usuarioRepository.ObtenerRolesPorIdsAsync(rolesIds, cancellationToken);

            var idsEncontrados = roles.Select(rol => rol.Id).ToHashSet();
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
    }
}
