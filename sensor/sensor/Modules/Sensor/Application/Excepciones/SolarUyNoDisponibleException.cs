using System;

namespace Modules.Sensor.Application.Excepciones
{
    public class SolarUyNoDisponibleException : Exception
    {
        public SolarUyNoDisponibleException(string mensaje) : base(mensaje)
        {
        }

        public SolarUyNoDisponibleException(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}
