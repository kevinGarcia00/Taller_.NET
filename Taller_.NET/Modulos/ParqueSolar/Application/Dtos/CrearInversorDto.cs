namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class CrearInversorDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int NodoId { get; set; }
        public int ParqueSolarId { get; set; }
    }
}
