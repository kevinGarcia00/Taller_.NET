using System;

namespace Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones
{
    public class PanelSinCambiosException : Exception
    {
        public PanelSinCambiosException(string mensaje) : base(mensaje)
        {
        }
    }
}
