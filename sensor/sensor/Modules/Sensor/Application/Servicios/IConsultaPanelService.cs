using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Dtos;

namespace Modules.Sensor.Application.Servicios
{
    public interface IConsultaPanelService
    {
        Task<IReadOnlyList<InversorListadoDto>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<NodoListadoDto>> ObtenerNodosInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesNodoInversorAsync(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken = default);

        Task<PanelDetalleDto> ObtenerPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CancellationToken cancellationToken = default);
    }
}
