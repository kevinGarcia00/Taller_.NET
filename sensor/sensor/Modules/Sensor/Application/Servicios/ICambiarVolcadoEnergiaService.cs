using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Dtos;

namespace Modules.Sensor.Application.Servicios
{
    public interface ICambiarVolcadoEnergiaService
    {
        Task<PanelDetalleDto> CambiarVolcadoEnergiaAsync(
            CambiarVolcadoEnergiaDto dto,
            CancellationToken cancellationToken = default);
    }
}
