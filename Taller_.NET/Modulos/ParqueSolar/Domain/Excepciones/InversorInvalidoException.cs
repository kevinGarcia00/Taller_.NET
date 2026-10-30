using System;

namespace Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones
{
    public class InversorInvalidoException : Exception
    {
        public InversorInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
