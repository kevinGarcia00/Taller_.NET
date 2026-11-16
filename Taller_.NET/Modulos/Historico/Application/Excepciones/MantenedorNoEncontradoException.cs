using System;

namespace Taller_.NET.Modulos.Historico.Application.Excepciones
{
    public class MantenedorNoEncontradoException : Exception
    {
        public MantenedorNoEncontradoException()
            : base("El usuario de mantenimiento no existe.")
        {
        }

        public MantenedorNoEncontradoException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
