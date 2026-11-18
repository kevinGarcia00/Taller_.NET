using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Seguridad
{
    public class UsuarioActualProvider : IUsuarioActualProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsuarioActualProvider(
            IHttpContextAccessor httpContextAccessor,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> TienePermisoAsync(string permiso, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(permiso))
            {
                return false;
            }

            var usuario = await ObtenerUsuarioAutenticadoAsync();

            if (usuario.Identity?.IsAuthenticated != true)
            {
                return false;
            }

            return ObtenerPermisos(usuario).Contains(permiso, StringComparer.Ordinal);
        }

        public async Task<string?> ObtenerAuth0UserIdAsync(CancellationToken cancellationToken = default)
        {
            var usuario = await ObtenerUsuarioAutenticadoAsync();

            if (usuario.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            // Auth0 expone el identificador en "sub"; segun MapInboundClaims y la
            // configuracion del token validation puede llegar mapeado a nameidentifier.
            return usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? usuario.FindFirst("sub")?.Value
                ?? usuario.FindFirst("user_id")?.Value;
        }

        public async Task<string?> ObtenerCorreoAsync(CancellationToken cancellationToken = default)
        {
            var usuario = await ObtenerUsuarioAutenticadoAsync();

            if (usuario.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            return usuario.FindFirst(ClaimTypes.Email)?.Value
                ?? usuario.FindFirst("email")?.Value;
        }

        private static IEnumerable<string> ObtenerPermisos(ClaimsPrincipal usuario)
        {
            foreach (var claim in usuario.FindAll(PermisosUsuarios.TipoClaim))
            {
                if (claim.Value.StartsWith('['))
                {
                    foreach (var permiso in ObtenerPermisosDeArregloJson(claim.Value))
                    {
                        yield return permiso;
                    }

                    continue;
                }

                yield return claim.Value;
            }
        }

        private static IEnumerable<string> ObtenerPermisosDeArregloJson(string valor)
        {
            List<string> permisos;

            try
            {
                permisos = JsonSerializer.Deserialize<List<string>>(valor) ?? new List<string>();
            }
            catch (JsonException)
            {
                return Array.Empty<string>();
            }

            return permisos.Where(permiso => !string.IsNullOrWhiteSpace(permiso));
        }

        private async Task<ClaimsPrincipal> ObtenerUsuarioAutenticadoAsync()
        {
            var contextoHttp = _httpContextAccessor.HttpContext;

            // En controllers y APIs el principal lo arma el middleware de autenticacion.
            // AuthenticationStateProvider pertenece al scope de un componente Blazor:
            // llamarlo desde una peticion HTTP lanza "Do not call GetAuthenticationStateAsync
            // outside of the DI scope for a Razor component".
            if (contextoHttp is not null)
            {
                return contextoHttp.User;
            }

            // Sin HttpContext estamos dentro del circuito interactivo de Blazor.
            var estadoAutenticacion = await _authenticationStateProvider.GetAuthenticationStateAsync();

            return estadoAutenticacion.User;
        }
    }
}
