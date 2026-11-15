using System;

namespace Taller_.NET.Modulos.Historico.Application.Dtos
{
    public class HistoricoMantenibilidadListadoDto
    {
        public int Id { get; set; }
        public Guid IdMantenedor { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public string EstadoInicial { get; set; } = string.Empty;
        public string EstadoResultante { get; set; } = string.Empty;
    }
}
