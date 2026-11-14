using Taller_.NET.Modulos.Historico.Domain.Excepciones;

namespace Taller_.NET.Modulos.Historico.Domain
{
    /// <summary>
    /// Registro de la energia total de un panel en una fecha determinada.
    /// </summary>
    public class Historico
    {
        private ContextoPanel? _contextoPanel;

        public int Id { get; private set; }
        public DateTime Fecha { get; private set; }
        public decimal EnergiaTotal { get; private set; }
        public int PanelId { get; private set; }
        public int NodoId { get; private set; }
        public int ParqueSolarId { get; private set; }

        public ContextoPanel ContextoPanel =>
            _contextoPanel ??= new ContextoPanel(PanelId, NodoId, ParqueSolarId);

        private Historico()
        {
        }

        public Historico(DateTime fecha, decimal energiaTotal, ContextoPanel contextoPanel)
        {
            if (energiaTotal < 0)
            {
                throw new HistoricoInvalidoException("La energia total del registro no puede ser negativa.");
            }

            if (fecha > DateTime.UtcNow)
            {
                throw new HistoricoInvalidoException("La fecha del registro no puede ser futura.");
            }

            if (contextoPanel is null)
            {
                throw new HistoricoInvalidoException("El contexto del panel es obligatorio.");
            }

            Fecha = fecha;
            EnergiaTotal = energiaTotal;
            PanelId = contextoPanel.PanelId;
            NodoId = contextoPanel.NodoId;
            ParqueSolarId = contextoPanel.ParqueSolarId;
            _contextoPanel = contextoPanel;
        }
    }
}
