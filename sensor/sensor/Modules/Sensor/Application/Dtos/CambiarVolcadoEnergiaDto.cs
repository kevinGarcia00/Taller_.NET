namespace Modules.Sensor.Application.Dtos
{
    public class CambiarVolcadoEnergiaDto
    {
        public int IdInversor { get; set; }
        public int IdNodo { get; set; }
        public int IdPanel { get; set; }
        public bool VolcarEnergia { get; set; }
    }
}
