namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class CrearNodoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string UbicacionParque { get; set; } = string.Empty;
        public int? InversorId { get; set; }
        public int ParqueSolarId { get; set; }
    }
}
