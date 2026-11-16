using System.Security.Claims;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;

namespace Taller_.NET.Modulos.Usuario.Application.Servicios
{
    public class UsuarioAutenticacionService : IUsuarioAutenticacionService
    {
        private readonly IAuth0AdministracionProvider _auth0AdministracionProvider;
        private readonly ILogger<UsuarioAutenticacionService> _logger;

        public UsuarioAutenticacionService(
            IAuth0AdministracionProvider auth0AdministracionProvider,
            ILogger<UsuarioAutenticacionService> logger)
        {
            _auth0AdministracionProvider = auth0AdministracionProvider;
            _logger = logger;
        }

        public async Task<bool> AgregarDatosDeAutorizacionAsync(
            ClaimsPrincipal principal,
            string? accessToken,
            CancellationToken cancellationToken = default)
        {
            if (principal is null)
            {
                return false;
            }

            var identidad = principal.Identities.FirstOrDefault(identidad => identidad.IsAuthenticated);

            if (identidad is null)
            {
                return false;
            }

            AgregarPermisosDelAccessToken(identidad, accessToken);

            var auth0UserId = ObtenerAuth0UserId(principal);

            if (string.IsNullOrWhiteSpace(auth0UserId))
            {
                return false;
            }

            await AgregarRolesDeAuth0Async(identidad, auth0UserId, cancellationToken);

            return true;
        }

        private static void AgregarPermisosDelAccessToken(ClaimsIdentity identidad, string? accessToken)
        {
            foreach (var permiso in PermisosDeAccessToken.Obtener(accessToken))
            {
                if (!identidad.HasClaim(PermisosUsuarios.TipoClaim, permiso))
                {
                    identidad.AddClaim(new Claim(PermisosUsuarios.TipoClaim, permiso));
                }
            }
        }

        private async Task AgregarRolesDeAuth0Async(
            ClaimsIdentity identidad,
            string auth0UserId,
            CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _auth0AdministracionProvider.ObtenerRolesDeUsuarioAsync(
                    auth0UserId,
                    cancellationToken);

                foreach (var rol in roles)
                {
                    if (!identidad.HasClaim(ClaimTypes.Role, rol.Nombre))
                    {
                        identidad.AddClaim(new Claim(ClaimTypes.Role, rol.Nombre));
                    }
                }

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var rolesAplicados = roles.Count == 0
                        ? "(ninguno)"
                        : string.Join(", ", roles.Select(rol => rol.Nombre));

                    _logger.LogInformation(
                        "Roles de Auth0 aplicados al usuario {Auth0UserId}: {Roles}",
                        auth0UserId,
                        rolesAplicados);
                }
            }
            catch (Exception excepcion)
            {
                // Un fallo al consultar los roles en Auth0 no debe impedir el inicio
                // de sesion: los permisos del Access Token siguen siendo la fuente
                // de autorizacion de las politicas del modulo.
                _logger.LogWarning(
                    excepcion,
                    "No se pudieron obtener los roles de Auth0 para el usuario {Auth0UserId}.",
                    auth0UserId);
            }
        }

        private static string? ObtenerAuth0UserId(ClaimsPrincipal principal)
        {

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst("sub")?.Value
                ?? principal.FindFirst("user_id")?.Value;
        }
    }
}
