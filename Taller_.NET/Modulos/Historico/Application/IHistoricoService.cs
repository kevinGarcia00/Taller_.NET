using Taller_.NET.Modulos.Historico.Application.Dtos;

namespace Taller_.NET.Modulos.Historico.Application
{
    public interface IHistoricoService
    {
        Task<int> GenerarRegistrosDelPeriodoAsync(
            TimeSpan intervalo,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<HistoricoListadoDto>> ObtenerRegistrosPorPanelAsync(
            int panelId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<HistoricoListadoDto>> ObtenerRegistrosFiltradosPorPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            FiltroHistoricoDto filtro,
            CancellationToken cancellationToken = default);
    }
}
