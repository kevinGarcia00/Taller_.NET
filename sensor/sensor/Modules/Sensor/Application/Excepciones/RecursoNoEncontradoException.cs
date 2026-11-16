using System;

namespace Modules.Sensor.Application.Excepciones
{
    public class RecursoNoEncontradoException : Exception
    {
        public RecursoNoEncontradoException(string mensaje) : base(mensaje)
        {
        }
    }
}
