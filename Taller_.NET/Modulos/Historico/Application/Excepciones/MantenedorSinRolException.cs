using System;

namespace Taller_.NET.Modulos.Historico.Application.Excepciones
{
    public class MantenedorSinRolException : Exception
    {
        public MantenedorSinRolException()
            : base("El usuario seleccionado no tiene el rol de Mantenimiento.")
        {
        }
    }
}
