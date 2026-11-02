namespace Taller_.NET.Modulos.Usuario.Application.Dtos
{
    public class ModificarUsuarioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Identificadores (role_id) de los roles de Auth0 que tendra el usuario.
        /// </summary>
        public List<string> RolesIds { get; set; } = new();
    }
}
