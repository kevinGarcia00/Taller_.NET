using Taller_.NET.Modulos.Historico.Domain;

namespace Taller_.NET.Modulos.Historico.Application.Dtos
{
    public class ContextoPanelLecturaDto
    {
        public string NombreParque { get; set; } = string.Empty;
        public string NombreNodo { get; set; } = string.Empty;
        public string NombrePanel { get; set; } = string.Empty;
        public EstadoPanelRegistrado Estado { get; set; }
    }
}
