using Taller_.NET.Modulos.Historico.Domain.Excepciones;

namespace Taller_.NET.Modulos.Historico.Domain
{
    public class ContextoPanel
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
                throw new HistoricoMantenibilidadInvalidoException("El panel del registro es obligatorio.");
            }

            if (nodoId <= 0)
            {
                throw new HistoricoMantenibilidadInvalidoException("El nodo del registro es obligatorio.");
            }

            if (parqueSolarId <= 0)
            {
                throw new HistoricoMantenibilidadInvalidoException("El parque solar del registro es obligatorio.");
            }

            PanelId = panelId;
            NodoId = nodoId;
            ParqueSolarId = parqueSolarId;
        }
    }
}
