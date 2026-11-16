using System;

namespace Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones
{
    public class VolcadoEnergiaSinCambiosException : Exception
    {
        public VolcadoEnergiaSinCambiosException(string mensaje) : base(mensaje)
        {
        }
    }
}
