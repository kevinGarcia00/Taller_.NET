using System;
using Taller_.NET.Modulos.Historico.Domain;

namespace Taller_.NET.Modulos.Historico.Application.Dtos
{
    public class CrearHistoricoMantenibilidadDto
    {
        public Guid IdMantenedor { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoPanelRegistrado EstadoResultante { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int PanelId { get; set; }
        public int NodoId { get; set; }
        public int ParqueSolarId { get; set; }
    }
}
