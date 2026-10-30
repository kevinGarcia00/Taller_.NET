using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class NodoNoEncontradoException : Exception
    {
        public NodoNoEncontradoException(int nodoId)
            : base($"No se encontro un nodo con el identificador {nodoId}.")
        {
        }
    }
}
