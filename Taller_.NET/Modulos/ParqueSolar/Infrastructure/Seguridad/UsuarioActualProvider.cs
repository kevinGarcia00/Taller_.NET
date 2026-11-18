using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Seguridad
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

        public async Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default)
        {
            var usuario = await ObtenerUsuarioActualAsync();

            return usuario.Identity?.IsAuthenticated == true && usuario.IsInRole("Administrador");
        }

        public async Task<bool> EsMantenimientoAsync(CancellationToken cancellationToken = default)
        {
            var usuario = await ObtenerUsuarioActualAsync();

            return usuario.Identity?.IsAuthenticated == true && usuario.IsInRole("Mantenimiento");
        }

        // En controllers/APIs el usuario viene del HttpContext; AuthenticationStateProvider
        // solo se puede usar dentro del scope de un componente Blazor.
        private async Task<ClaimsPrincipal> ObtenerUsuarioActualAsync()
        {
            var contextoHttp = _httpContextAccessor.HttpContext;

            if (contextoHttp is not null)
            {
                return contextoHttp.User;
            }

            var estadoAutenticacion = await _authenticationStateProvider.GetAuthenticationStateAsync();

            return estadoAutenticacion.User;
        }
    }
}
