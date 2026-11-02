namespace Taller_.NET.Modulos.Usuario.Application.Puertos
{
    public interface IUsuarioActualProvider
    {
        /// <summary>
        /// Indica si el usuario autenticado tiene el permiso indicado en el claim
        /// "permissions" del Access Token emitido por Auth0.
        /// </summary>
        Task<bool> TienePermisoAsync(string permiso, CancellationToken cancellationToken = default);

        /// <summary>
        /// Identificador del usuario autenticado en Auth0 (claim sub/nameidentifier).
        /// Devuelve null si no hay una sesion autenticada o si el esquema no lo expone.
        /// </summary>
        Task<string?> ObtenerAuth0UserIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Correo del usuario autenticado (claim email). Devuelve null si no esta disponible.
        /// </summary>
        Task<string?> ObtenerCorreoAsync(CancellationToken cancellationToken = default);
    }
}
