using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Dtos;

namespace Modules.Sensor.Application.Servicios
{
    public interface ICambiarEstadoPanelService
    {
        Task<PanelDetalleDto> CambiarEstadoPanelAsync(
            CambiarEstadoPanelDto dto,
            CancellationToken cancellationToken = default);
    }
}
