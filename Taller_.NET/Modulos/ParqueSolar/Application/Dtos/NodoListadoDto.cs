using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class NodoListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string UbicacionParque { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public decimal EnergiaGenerada { get; set; }
        public int? InversorId { get; set; }
        public string? NombreInversor { get; set; }
        public int ParqueSolarId { get; set; }
    }
}
