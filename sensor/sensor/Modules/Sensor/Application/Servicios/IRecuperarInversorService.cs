using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Dtos;

namespace Modules.Sensor.Application.Servicios
{
    public interface IRecuperarInversorService
    {
        Task<InversorDetalleDto> RecuperarInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default);
    }
}
