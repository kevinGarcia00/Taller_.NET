using System;

namespace Modules.Sensor.Application.Puertos
{
    public interface IProveedorTiempoSensor
    {
        DateTime ObtenerFechaActualUtc();

        TimeSpan ObtenerUptime();
    }
}
