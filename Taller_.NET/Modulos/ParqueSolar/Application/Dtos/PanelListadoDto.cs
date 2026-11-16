namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class PanelListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool VolcarEnergia { get; set; }
        public decimal Potencia { get; set; }
    }
}
