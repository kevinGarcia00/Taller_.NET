using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface IConsultaInversorService
    {
        Task<IReadOnlyList<InversorConsultaDto>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default);

        Task<InversorConsultaDto?> ObtenerInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<NodoConsultaDto>> ObtenerNodosInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PanelConsultaDto>> ObtenerPanelesNodoInversorAsync(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken = default);

        Task<PanelConsultaDto?> ObtenerPanelInversorAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CancellationToken cancellationToken = default);
    }
}
