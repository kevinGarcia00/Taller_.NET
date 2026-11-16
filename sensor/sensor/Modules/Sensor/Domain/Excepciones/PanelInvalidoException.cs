using System;

namespace Modules.Sensor.Domain.Excepciones
{
    public class PanelInvalidoException : Exception
    {
        public PanelInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
