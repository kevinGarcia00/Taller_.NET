namespace Taller_.NET.Modulos.Usuario.Application.Dtos
{
    /// <summary>
    /// Rol administrado por Auth0. El identificador es el role_id del tenant.
    /// </summary>
    public class RolDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }
}
