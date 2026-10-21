using System;
using System.Collections.Generic;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class ParqueDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public int CantMaxNodos { get; set; }
        public int CantidadNodosActuales { get; set; }
        public DateTime FechaCreacion { get; set; }
        public IReadOnlyList<NodoResumenDto> ListaNodos { get; set; } = new List<NodoResumenDto>();
    }
}
