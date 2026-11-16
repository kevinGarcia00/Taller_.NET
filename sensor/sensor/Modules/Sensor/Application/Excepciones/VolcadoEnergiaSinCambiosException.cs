using System;

namespace Modules.Sensor.Application.Excepciones
{
    public class VolcadoEnergiaSinCambiosException : Exception
    {
        public VolcadoEnergiaSinCambiosException(string mensaje) : base(mensaje)
        {
        }
    }
}
