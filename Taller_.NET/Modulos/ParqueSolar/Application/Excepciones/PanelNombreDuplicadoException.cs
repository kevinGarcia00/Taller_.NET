using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class PanelNombreDuplicadoException : Exception
    {
        public PanelNombreDuplicadoException(string nombre)
            : base($"Ya existe un panel registrado con el nombre '{nombre}' en el nodo seleccionado.")
        {
        }
    }
}
