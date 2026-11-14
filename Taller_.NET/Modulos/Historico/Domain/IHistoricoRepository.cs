namespace Taller_.NET.Modulos.Historico.Domain
{
    public interface IHistoricoRepository
    {
        Task AgregarAsync(Historico registro, CancellationToken cancellationToken = default);

        Task<bool> ExisteRegistroDelPanelEnPeriodoAsync(
            int panelId,
            DateTime periodoInicio,
            DateTime periodoFin,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Historico>> ObtenerPorPanelAsync(
            int panelId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Historico>> ObtenerPorPanelYFechaAsync(
            int panelId,
            DateTime fecha,
            CancellationToken cancellationToken = default);

        Task GuardarCambiosAsync(CancellationToken cancellationToken = default);
    }
}
