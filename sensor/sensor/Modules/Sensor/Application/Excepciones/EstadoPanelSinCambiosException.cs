using System;

namespace Modules.Sensor.Application.Excepciones
{
    public class EstadoPanelSinCambiosException : Exception
    {
        public EstadoPanelSinCambiosException(string mensaje) : base(mensaje)
        {
        }
    }
}
