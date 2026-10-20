using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class UsuarioNoAutorizadoException : Exception
    {
        public UsuarioNoAutorizadoException(string mensaje) : base(mensaje)
        {
        }
    }
}
