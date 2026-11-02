using Taller_.NET.Modulos.Usuario.Application.Dtos;

namespace Taller_.NET.Modulos.Usuario.Application.Puertos
{
    /// <summary>
    /// Puerto de integracion con la Management API de Auth0. Auth0 es la fuente
    /// unica de identidad, roles y permisos; la base local solo conserva los datos
    /// propios del negocio.
    /// </summary>
    public interface IAuth0AdministracionProvider
    {
        /// <summary>
        /// Crea el usuario en Auth0 y devuelve su identificador (user_id).
        /// </summary>
        Task<string> CrearUsuarioAsync(
            string correo,
            string contrasena,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza los datos personales del usuario en Auth0.
        /// </summary>
        Task ActualizarUsuarioAsync(
            string auth0UserId,
            string correo,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina el usuario de Auth0. Si ya no existe, la operacion se considera exitosa.
        /// </summary>
        Task EliminarUsuarioAsync(string auth0UserId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene los roles definidos en el tenant de Auth0.
        /// </summary>
        Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene los roles asignados a un usuario en Auth0.
        /// </summary>
        Task<List<RolDto>> ObtenerRolesDeUsuarioAsync(
            string auth0UserId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Reemplaza los roles asignados a un usuario en Auth0.
        /// </summary>
        Task ReemplazarRolesAsync(
            string auth0UserId,
            IReadOnlyCollection<string> rolesIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Cuenta los usuarios que tienen asignado el rol indicado en Auth0.
        /// </summary>
        Task<int> ObtenerCantidadUsuariosConRolAsync(
            string nombreRol,
            CancellationToken cancellationToken = default);
    }
}
