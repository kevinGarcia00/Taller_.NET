using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface INodoService
    {
        Task<int> CrearNodoAsync(CrearNodoDto dto, CancellationToken cancellationToken = default);

        Task<EditarNodoDto> ObtenerNodoParaEdicionAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<InversorOpcionDto>> ObtenerInversoresParaEdicionAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task ModificarNodoAsync(EditarNodoDto dto, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<NodoListadoDto>> ObtenerNodosPorParqueAsync(
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task EliminarNodoAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<InversorDisponibleDto>> ObtenerInversoresDisponiblesAsync(
            CancellationToken cancellationToken = default);
    }
}
