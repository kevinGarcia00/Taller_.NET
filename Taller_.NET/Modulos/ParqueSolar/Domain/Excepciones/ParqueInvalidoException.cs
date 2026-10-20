using System;

namespace Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones
{
    public class ParqueInvalidoException : Exception
    {
        public ParqueInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
