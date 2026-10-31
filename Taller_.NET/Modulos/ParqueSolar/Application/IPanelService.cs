using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface IPanelService
    {
        Task<int> CrearPanelAsync(CrearPanelDto dto, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesPorNodoAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<PanelDetalleDto> ObtenerPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task EliminarPanelAsync(
            int id,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<ModificarPanelDto> ObtenerParaEdicionAsync(
            int id,
            int nodoId,
            CancellationToken cancellationToken = default);

        Task ModificarPanelAsync(ModificarPanelDto dto, CancellationToken cancellationToken = default);
    }
}
