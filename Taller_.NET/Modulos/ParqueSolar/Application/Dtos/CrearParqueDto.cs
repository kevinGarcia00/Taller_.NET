namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class CrearParqueDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public int CantMaxNodos { get; set; }
    }
}
