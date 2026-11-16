using Taller_.NET.Modulos.Historico.Domain.Excepciones;

namespace Taller_.NET.Modulos.Historico.Domain
{
    /// <summary>
    /// Referencia propia del modulo Historico a un panel de ParqueSolar.
    /// Evita depender de la entidad Panel de otro modulo.
    /// </summary>
    public sealed class ContextoPanel
    {
        public int PanelId { get; private set; }
        public int NodoId { get; private set; }
        public int ParqueSolarId { get; private set; }

        private ContextoPanel()
        {
        }

        public ContextoPanel(int panelId, int nodoId, int parqueSolarId)
        {
            if (panelId <= 0)
            {
                throw new HistoricoInvalidoException("El panel del registro historico es obligatorio.");
            }

            if (nodoId <= 0)
            {
                throw new HistoricoInvalidoException("El nodo del registro historico es obligatorio.");
            }

            if (parqueSolarId <= 0)
            {
                throw new HistoricoInvalidoException("El parque solar del registro historico es obligatorio.");
            }

            PanelId = panelId;
            NodoId = nodoId;
            ParqueSolarId = parqueSolarId;
        }
    }
}
