namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class ParqueListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public int CantidadNodosActuales { get; set; }
    }
}
