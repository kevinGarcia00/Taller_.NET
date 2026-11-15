using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.Historico.Domain
{
    public interface IHistoricoMantenibilidadRepository
    {
        Task<int> AgregarAsync(
            HistoricoMantenibilidad registro,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<HistoricoMantenibilidad>> ObtenerPorPanelAsync(
            int panelId,
            CancellationToken cancellationToken = default);
    }
}
