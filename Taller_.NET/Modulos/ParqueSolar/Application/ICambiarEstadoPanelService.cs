using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface ICambiarEstadoPanelService
    {
        Task<PanelConsultaDto> CambiarEstadoPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CambiarEstadoPanelDto dto,
            CancellationToken cancellationToken = default);
    }
}
