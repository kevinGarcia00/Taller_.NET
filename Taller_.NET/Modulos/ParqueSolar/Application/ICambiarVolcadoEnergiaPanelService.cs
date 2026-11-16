using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface ICambiarVolcadoEnergiaPanelService
    {
        Task<PanelConsultaDto> CambiarVolcadoEnergiaPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CambiarVolcadoEnergiaPanelDto dto,
            CancellationToken cancellationToken = default);
    }
}
