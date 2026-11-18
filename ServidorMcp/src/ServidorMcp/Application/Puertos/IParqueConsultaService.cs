using Application.Dtos;

namespace Application.Puertos;

public interface IParqueConsultaService
{
    Task<IReadOnlyList<ParqueListadoDto>> ObtenerParquesAsync(CancellationToken cancellationToken = default);

    Task<ParqueDetalleDto> ObtenerDetallePorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    Task<ParqueListadoDto> ObtenerConMayorCapacidadAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NodoResumenDto>> ObtenerNodosPorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NodoResumenDto>> ObtenerNodosOrdenadosPorEnergiaAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NodoConCantidadPanelesDto>> ObtenerNodosConCantidadPanelesAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesAsync(
        string nombreNodo,
        string? nombreParque = null,
        CancellationToken cancellationToken = default);

    Task<int> ContarPanelesAsync(
        string nombreNodo,
        string? nombreParque = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesApagadosAsync(
        string nombreParque,
        CancellationToken cancellationToken = default);

    Task<PanelDetalleDto> ObtenerPanelPorNombreAsync(
        string nombrePanel,
        string? nombreParque = null,
        string? nombreNodo = null,
        CancellationToken cancellationToken = default);

    Task<InversorDetalleDto> ObtenerInversorDelNodoAsync(
        string nombreNodo,
        string? nombreParque = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InversorListadoDto>> ObtenerInversoresAsync(
        CancellationToken cancellationToken = default);
}
