namespace Taller_.NET.Modulos.Historico.Models
{
    public class HistoricoMantenibilidad
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdPanel { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public EstadoPanelRegistrado EstadoInicial { get; set; }
        public EstadoPanelRegistrado EstadoResultante { get; set; }

        public HistoricoMantenibilidad(int id, int idUsuario, int idPanel, DateTime fecha, string observaciones, EstadoPanelRegistrado estadoInicial, EstadoPanelRegistrado estadoResultante)
        {
            Id = id;
            IdUsuario = idUsuario;
            IdPanel = idPanel;
            Fecha = fecha;
            Observaciones = observaciones;
            EstadoInicial = estadoInicial;
            EstadoResultante = estadoResultante;
        }
    }

    public enum EstadoPanelRegistrado
    {
        SUCIO,
        ROTO,
        ENCENDIDO,
        DETERIORADO,
        APAGADO
    }
}
