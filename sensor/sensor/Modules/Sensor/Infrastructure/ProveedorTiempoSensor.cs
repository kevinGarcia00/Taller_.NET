using System;
using System.Diagnostics;
using Modules.Sensor.Application.Puertos;

namespace Modules.Sensor.Infrastructure
{
    public class ProveedorTiempoSensor : IProveedorTiempoSensor
    {
        private readonly Stopwatch _cronometro;

        public ProveedorTiempoSensor()
        {
            _cronometro = Stopwatch.StartNew();
        }

        public DateTime ObtenerFechaActualUtc()
        {
            return DateTime.UtcNow;
        }

        public TimeSpan ObtenerUptime()
        {
            return _cronometro.Elapsed;
        }
    }
}
