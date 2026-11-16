using System;

namespace Taller_.NET.Modulos.Historico.Domain.Excepciones
{
    public class HistoricoMantenibilidadInvalidoException : Exception
    {
        public HistoricoMantenibilidadInvalidoException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
