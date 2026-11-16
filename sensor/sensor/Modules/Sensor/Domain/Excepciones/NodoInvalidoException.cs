using System;

namespace Modules.Sensor.Domain.Excepciones
{
    public class NodoInvalidoException : Exception
    {
        public NodoInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
