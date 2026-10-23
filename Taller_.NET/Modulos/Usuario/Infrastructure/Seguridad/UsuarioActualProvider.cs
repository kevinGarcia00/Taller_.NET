using Microsoft.AspNetCore.Components.Authorization;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Puertos;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Seguridad
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

            return usuario.Identity?.IsAuthenticated == true && usuario.IsInRole(Rol.NombreAdministrador);
        }
    }
}
