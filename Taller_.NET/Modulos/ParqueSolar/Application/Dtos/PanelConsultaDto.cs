using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class PanelConsultaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string EstadoPanel { get; set; } = string.Empty;
        public bool Encendido { get; set; }
        public bool VolcarEnergia { get; set; }
        public decimal Potencia { get; set; }
        public string VidaUtil { get; set; } = string.Empty;
        public int IdInversor { get; set; }
        public int IdNodo { get; set; }
        public int IdParqueSolar { get; set; }
    }
}
