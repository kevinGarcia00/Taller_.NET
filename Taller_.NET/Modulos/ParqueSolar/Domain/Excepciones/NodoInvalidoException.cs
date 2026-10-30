using System;

namespace Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones
{
    public class NodoInvalidoException : Exception
    {
        public NodoInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
