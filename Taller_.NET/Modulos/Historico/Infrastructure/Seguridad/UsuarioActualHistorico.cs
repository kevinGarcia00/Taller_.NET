using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Taller_.NET.Modulos.Historico.Application.Puertos;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Seguridad
{
    public class UsuarioActualHistorico : IUsuarioActualHistorico
    {
        public const string RolMantenimiento = "Mantenimiento";
        public const string RolAdministrador = "Administrador";

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsuarioActualHistorico(AuthenticationStateProvider authenticationStateProvider)
        {
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
            var estado = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var usuario = estado.User;

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
            var estado = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var usuario = estado.User;

            return usuario.Identity?.IsAuthenticated == true && usuario.IsInRole(rol);
        }
    }
}
