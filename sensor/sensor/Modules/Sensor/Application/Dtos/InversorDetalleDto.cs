using System;

namespace Modules.Sensor.Application.Dtos
{
    public class InversorDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaConsulta { get; set; }
        public TimeSpan UptimeSensor { get; set; }
    }
}
