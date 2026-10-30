using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public sealed class ContextoParque
    {
        public int ParqueSolarId { get; private set; }

        private ContextoParque()
        {
        }

        public ContextoParque(int parqueSolarId)
        {
            if (parqueSolarId <= 0)
            {
                throw new NodoInvalidoException("El parque solar asociado al nodo es obligatorio.");
            }

            ParqueSolarId = parqueSolarId;
        }
    }
}
