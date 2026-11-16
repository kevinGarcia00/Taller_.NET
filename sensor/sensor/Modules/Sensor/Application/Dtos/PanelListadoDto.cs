namespace Modules.Sensor.Application.Dtos
{
    public class PanelListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string EstadoPanel { get; set; } = string.Empty;
        public bool Encendido { get; set; }
        public bool VolcarEnergia { get; set; }
        public decimal Potencia { get; set; }
    }
}
