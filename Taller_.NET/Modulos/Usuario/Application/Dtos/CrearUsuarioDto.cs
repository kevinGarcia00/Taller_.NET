namespace Taller_.NET.Modulos.Usuario.Application.Dtos
{
    public class CrearUsuarioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public List<long> RolesIds { get; set; } = new();
    }
}
