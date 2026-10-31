using System;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class ModificarPanelDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public EstadoPanel Estado { get; set; }
        public decimal Potencia { get; set; }
        public string VidaUtil { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int NodoId { get; set; }
        public int ParqueSolarId { get; set; }
    }
}
