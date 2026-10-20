using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Seguridad
{
    public class UsuarioActualProvider : IUsuarioActualProvider
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsuarioActualProvider(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default)
        {
            var estadoAutenticacion = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var usuario = estadoAutenticacion.User;

            return usuario.Identity?.IsAuthenticated == true && usuario.IsInRole("Administrador");
        }
    }
}
