using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class ModificarInversorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int NodoId { get; set; }
        public int ParqueSolarId { get; set; }
    }
}
