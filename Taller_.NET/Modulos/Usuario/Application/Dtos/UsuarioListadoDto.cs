namespace Taller_.NET.Modulos.Usuario.Application.Dtos
{
    public class UsuarioListadoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
