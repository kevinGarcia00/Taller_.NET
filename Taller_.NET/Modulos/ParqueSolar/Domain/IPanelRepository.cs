using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public interface IPanelRepository
    {
        Task<bool> ExistePanelConNombreAsync(
            string nombre,
            int nodoId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Panel>> ObtenerPorNodoAsync(
            int nodoId,
            CancellationToken cancellationToken = default);

        Task<Panel?> ObtenerPorIdYNodoAsync(
            int panelId,
            int nodoId,
            CancellationToken cancellationToken = default);

        Task<bool> ExisteOtroPanelConNombreAsync(
            string nombre,
            int nodoId,
            int panelIdActual,
            CancellationToken cancellationToken = default);

        Task<int> AgregarAsync(Panel panel, CancellationToken cancellationToken = default);

        Task ActualizarAsync(Panel panel, CancellationToken cancellationToken = default);

        Task EliminarAsync(Panel panel, CancellationToken cancellationToken = default);
    }
}
