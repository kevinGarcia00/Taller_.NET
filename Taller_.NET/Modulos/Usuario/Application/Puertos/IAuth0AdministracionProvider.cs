namespace Taller_.NET.Modulos.Usuario.Application.Puertos
{
    /// <summary>
    /// Puerto de integracion con la Management API de Auth0: permite crear usuarios
    /// en el proveedor de identidad con sus roles.
    /// </summary>
    public interface IAuth0AdministracionProvider
    {
        Task<string> CrearUsuarioAsync(
            string correo,
            string contrasena,
            string nombre,
            string apellido,
            IEnumerable<string> roles,
            CancellationToken cancellationToken = default);
    }
}
