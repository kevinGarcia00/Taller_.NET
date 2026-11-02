using System.Security.Claims;

namespace Taller_.NET.Modulos.Usuario.Application.Puertos
{
    /// <summary>
    /// Completa el principal autenticado con la informacion de autorizacion que
    /// administra Auth0: roles del usuario y permisos del Access Token.
    /// </summary>
    public interface IUsuarioAutenticacionService
    {
        /// <summary>
        /// Agrega al principal los roles asignados en Auth0 y los permisos incluidos
        /// en el claim "permissions" del Access Token. Devuelve false si el principal
        /// no esta autenticado o no expone el identificador del usuario.
        /// </summary>
        Task<bool> AgregarDatosDeAutorizacionAsync(
            ClaimsPrincipal principal,
            string? accessToken,
            CancellationToken cancellationToken = default);
    }
}
