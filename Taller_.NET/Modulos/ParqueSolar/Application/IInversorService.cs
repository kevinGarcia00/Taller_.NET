using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface IInversorService
    {
        Task<int> CrearInversorAsync(CrearInversorDto dto, CancellationToken cancellationToken = default);

        Task<InversorDetalleDto?> ObtenerInversorDelNodoAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<ModificarInversorDto> ObtenerParaEdicionAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task ModificarInversorAsync(ModificarInversorDto dto, CancellationToken cancellationToken = default);

        Task EliminarInversorAsync(int nodoId, int parqueSolarId, CancellationToken cancellationToken = default);
    }
}
