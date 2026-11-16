using Modules.Sensor.Domain;

namespace Modules.Sensor.Application.Dtos
{
    public class CambiarEstadoPanelDto
    {
        public int IdInversor { get; set; }
        public int IdNodo { get; set; }
        public int IdPanel { get; set; }
        public AccionEstadoPanel Accion { get; set; }
    }
}
