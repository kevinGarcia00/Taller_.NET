using System;

namespace Modules.Sensor.Domain.Excepciones
{
    public class InversorInvalidoException : Exception
    {
        public InversorInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
