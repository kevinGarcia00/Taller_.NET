using System;

namespace Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones
{
    public class PanelInvalidoException : Exception
    {
        public PanelInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
