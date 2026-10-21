using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class ParqueNoEncontradoException : Exception
    {
        public ParqueNoEncontradoException(int parqueSolarId)
            : base($"No se encontro un parque solar con el identificador {parqueSolarId}.")
        {
        }
    }
}
