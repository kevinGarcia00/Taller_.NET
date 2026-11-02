namespace Taller_.NET.Modulos.Usuario.Application.Dtos
{
    public class CrearUsuarioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Contrasena inicial del usuario. Se envia a Auth0 y nunca se persiste
        /// en la base de datos de la aplicacion.
        /// </summary>
        public string Contrasena { get; set; } = string.Empty;

        /// <summary>
        /// Identificadores (role_id) de los roles de Auth0 que se asignaran al usuario.
        /// </summary>
        public List<string> RolesIds { get; set; } = new();
    }
}
