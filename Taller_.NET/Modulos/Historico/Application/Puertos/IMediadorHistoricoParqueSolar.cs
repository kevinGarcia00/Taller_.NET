using Taller_.NET.Modulos.Historico.Application.Dtos;

namespace Taller_.NET.Modulos.Historico.Application.Puertos
{
    /// <summary>
    /// Canal de comunicacion entre el modulo Historico y el modulo ParqueSolar.
    /// Historico solo conoce este puerto; el adaptador que conoce ParqueSolar
    /// se registra en el punto de composicion.
    /// </summary>
    public interface IMediadorHistoricoParqueSolar
    {
        Task<IReadOnlyList<EnergiaPanelDto>> ObtenerEnergiaTotalPanelesAsync(
            CancellationToken cancellationToken = default);

        Task<bool> ExistePanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);
    }
}
