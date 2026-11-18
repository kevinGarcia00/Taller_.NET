using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;

namespace Taller_.NET.Components.Pages.Usuarios
{
    public abstract class PaginaUsuariosBase : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

        protected async Task<bool> PuedeVerInterfazAsync()
        {
            if (AuthenticationStateTask is null)
            {
                return false;
            }

            var estado = await AuthenticationStateTask;

            return estado.User.Identity?.IsAuthenticated == true
                && (!estado.User.IsInRole(RolesAuth0.Mantenimiento)
                    || estado.User.IsInRole(RolesAuth0.Administrador));
        }
    }
}
