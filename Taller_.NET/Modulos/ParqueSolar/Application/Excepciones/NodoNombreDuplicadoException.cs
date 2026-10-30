using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class NodoNombreDuplicadoException : Exception
    {
        public NodoNombreDuplicadoException(string nombre)
            : base($"Ya existe un nodo registrado con el nombre '{nombre}' en el parque solar seleccionado.")
        {
        }
    }
}
