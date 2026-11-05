using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class NodoYaTieneInversorException : Exception
    {
        public NodoYaTieneInversorException()
            : base("El nodo ya tiene un inversor asignado.")
        {
        }
    }
}
