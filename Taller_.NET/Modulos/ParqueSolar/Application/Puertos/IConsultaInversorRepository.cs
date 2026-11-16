using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Puertos
{
    public interface IConsultaInversorRepository
    {
        Task<IReadOnlyList<InversorDomain>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default);

        Task<InversorDomain?> ObtenerInversorPorIdAsync(
            int idInversor,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<NodoDomain>> ObtenerNodosPorInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default);

        Task<NodoDomain?> ObtenerNodoPorIdAsync(
            int idNodo,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PanelDomain>> ObtenerPanelesPorNodoAsync(
            int idNodo,
            CancellationToken cancellationToken = default);

        Task<PanelDomain?> ObtenerPanelPorIdYNodoAsync(
            int idPanel,
            int idNodo,
            CancellationToken cancellationToken = default);
    }
}
