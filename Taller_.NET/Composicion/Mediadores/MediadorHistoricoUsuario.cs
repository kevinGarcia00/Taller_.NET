using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;

namespace Taller_.NET.Composicion.Mediadores
{
    public class MediadorHistoricoUsuario : IMediadorHistoricoUsuario
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuth0AdministracionProvider _auth0;
        private readonly ILogger<MediadorHistoricoUsuario> _logger;

        public MediadorHistoricoUsuario(
            IUsuarioRepository usuarioRepository,
            IAuth0AdministracionProvider auth0,
            ILogger<MediadorHistoricoUsuario> logger)
        {
            _usuarioRepository = usuarioRepository;
            _auth0 = auth0;
            _logger = logger;
        }

        public async Task<ResultadoMantenedor> ConfirmarMantenedorAsync(
            Guid idMantenedor,
            CancellationToken cancellationToken = default)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(idMantenedor, cancellationToken);

            if (usuario is null)
            {
                return ResultadoMantenedor.Inexistente;
            }

            if (!await TieneRolMantenimientoAsync(usuario.Auth0UserId, cancellationToken))
            {
                return ResultadoMantenedor.SinRol;
            }

            return ResultadoMantenedor.Valido;
        }

        public async Task<MantenedorOpcionDto?> ObtenerMantenedorPorAuth0Async(
            string? auth0UserId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(auth0UserId))
            {
                return null;
            }

            var usuario = await _usuarioRepository.ObtenerPorAuth0UserIdAsync(auth0UserId, cancellationToken);

            if (usuario is null)
            {
                return null;
            }

            return new MantenedorOpcionDto
            {
                Id = usuario.Id,
                Nombre = $"{usuario.Nombre} {usuario.Apellido}".Trim()
            };
        }

        private async Task<bool> TieneRolMantenimientoAsync(
            string? auth0UserId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(auth0UserId))
            {
                return false;
            }

            try
            {
                var roles = await _auth0.ObtenerRolesDeUsuarioAsync(auth0UserId, cancellationToken);

                return roles.Any(rol =>
                    string.Equals(rol.Nombre, RolesAuth0.Mantenimiento, StringComparison.OrdinalIgnoreCase));
            }
            catch (UsuarioAuth0NoEncontradoException)
            {
                _logger.LogWarning(
                    "Se omitio el usuario {Auth0UserId} al buscar mantenedores porque ya no existe en Auth0.",
                    auth0UserId);

                return false;
            }
        }
    }
}
