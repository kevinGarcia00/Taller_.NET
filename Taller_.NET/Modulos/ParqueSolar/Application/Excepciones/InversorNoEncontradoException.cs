using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class InversorNoEncontradoException : Exception
    {
        public InversorNoEncontradoException(int inversorId)
            : base($"No existe un inversor con el identificador '{inversorId}'.")
        {
        }
    }
}
