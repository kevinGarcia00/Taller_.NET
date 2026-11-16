using System;
using Taller_.NET.Modulos.Historico.Domain.Excepciones;

namespace Taller_.NET.Modulos.Historico.Domain
{
    public class Mantenedor
    {
        public Guid IdMantenedor { get; private set; }

        private Mantenedor()
        {
        }

        public Mantenedor(Guid idMantenedor)
        {
            if (idMantenedor == Guid.Empty)
            {
                throw new HistoricoMantenibilidadInvalidoException("El mantenedor es obligatorio.");
            }

            IdMantenedor = idMantenedor;
        }
    }
}
