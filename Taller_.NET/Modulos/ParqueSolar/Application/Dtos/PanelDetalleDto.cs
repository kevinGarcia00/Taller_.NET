using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class PanelDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal Potencia { get; set; }
        public string VidaUtil { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int NodoId { get; set; }
        public int ParqueSolarId { get; set; }
    }
}
