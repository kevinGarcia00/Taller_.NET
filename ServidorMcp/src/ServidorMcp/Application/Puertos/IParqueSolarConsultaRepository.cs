using Application.Dtos;

namespace Application.Puertos;

public interface IParqueSolarConsultaRepository
{
    Task<IReadOnlyList<ParqueListadoDto>> ObtenerParquesAsync(CancellationToken cancellationToken = default);

    Task<ParqueDetalleDto?> ObtenerDetalleParqueAsync(int parqueSolarId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NodoListadoDto>> ObtenerNodosPorParqueAsync(
        int parqueSolarId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesPorNodoAsync(
        int parqueSolarId,
        int nodoId,
        CancellationToken cancellationToken = default);

    Task<PanelDetalleDto?> ObtenerPanelAsync(
        int parqueSolarId,
        int nodoId,
        int panelId,
        CancellationToken cancellationToken = default);

    Task<InversorDetalleDto?> ObtenerInversorDelNodoAsync(
        int parqueSolarId,
        int nodoId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InversorListadoDto>> ObtenerInversoresAsync(CancellationToken cancellationToken = default);
}
