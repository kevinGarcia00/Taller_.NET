using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class InversorYaAsignadoException : Exception
    {
        public InversorYaAsignadoException(int inversorId)
            : base($"El inversor con el identificador '{inversorId}' ya esta asociado a otro nodo.")
        {
        }
    }
}
