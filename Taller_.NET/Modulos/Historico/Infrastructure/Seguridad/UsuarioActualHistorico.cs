using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Taller_.NET.Modulos.Historico.Application.Puertos;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Seguridad
{
    public class UsuarioActualHistorico : IUsuarioActualHistorico
    {
        public const string RolMantenimiento = "Mantenimiento";
        public const string RolAdministrador = "Administrador";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsuarioActualHistorico(
            IHttpContextAccessor httpContextAccessor,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public Task<bool> EsMantenimientoAsync(CancellationToken cancellationToken = default)
        {
            return TieneRolAsync(RolMantenimiento);
        }

        public Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default)
        {
            return TieneRolAsync(RolAdministrador);
        }

        public async Task<string?> ObtenerAuth0UserIdAsync(CancellationToken cancellationToken = default)
        {
            var usuario = await ObtenerUsuarioActualAsync();

            if (usuario.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            return usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? usuario.FindFirst("sub")?.Value
                ?? usuario.FindFirst("user_id")?.Value;
        }

        private async Task<bool> TieneRolAsync(string rol)
        {
            var usuario = await ObtenerUsuarioActualAsync();

            return usuario.Identity?.IsAuthenticated == true && usuario.IsInRole(rol);
        }

        // En peticiones HTTP se usa el principal del middleware; en un circuito
        // interactivo de Blazor se usa el AuthenticationStateProvider.
        private async Task<ClaimsPrincipal> ObtenerUsuarioActualAsync()
        {
            var contextoHttp = _httpContextAccessor.HttpContext;

            if (contextoHttp is not null)
            {
                return contextoHttp.User;
            }

            var estado = await _authenticationStateProvider.GetAuthenticationStateAsync();

            return estado.User;
        }
    }
}
