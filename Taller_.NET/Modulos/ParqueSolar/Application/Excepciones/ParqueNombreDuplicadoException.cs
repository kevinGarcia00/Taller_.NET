using System;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class ParqueNombreDuplicadoException : Exception
    {
        public ParqueNombreDuplicadoException(string nombre)
            : base($"Ya existe un parque solar registrado con el nombre '{nombre}'.")
        {
        }
    }
}
