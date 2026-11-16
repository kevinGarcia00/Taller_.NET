using System.Collections.Generic;

namespace Modules.Sensor.Application.Dtos
{
    public class NodoListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdInversor { get; set; }
        public IReadOnlyList<PanelListadoDto> Paneles { get; set; } = new List<PanelListadoDto>();
    }
}
