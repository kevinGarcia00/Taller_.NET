using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class InversorConsultaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }
}
