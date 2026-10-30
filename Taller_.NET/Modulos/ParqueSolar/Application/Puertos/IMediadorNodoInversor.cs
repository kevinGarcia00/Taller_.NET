using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Puertos
{
    public interface IMediadorNodoInversor
    {
        Task<bool> ExisteInversorAsync(int inversorId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<InversorDisponibleDto>> ObtenerDisponiblesAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<int, string>> ObtenerNombresPorIdsAsync(
            IReadOnlyCollection<int> inversorIds,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<InversorOpcionDto>> ObtenerOpcionesParaNodoAsync(
            int? inversorActualId,
            CancellationToken cancellationToken = default);
    }
}
