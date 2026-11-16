using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Dtos
{
    public class NodoConsultaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int IdInversor { get; set; }
        public int IdParqueSolar { get; set; }
    }
}
