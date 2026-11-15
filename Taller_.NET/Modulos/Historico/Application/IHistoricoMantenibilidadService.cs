using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.Historico.Application.Dtos;

namespace Taller_.NET.Modulos.Historico.Application
{
    public interface IHistoricoMantenibilidadService
    {
        Task<int> RegistrarRegistroAsync(
            CrearHistoricoMantenibilidadDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<HistoricoMantenibilidadListadoDto>> ObtenerRegistrosPorPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<ContextoPanelLecturaDto> ObtenerContextoPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<MantenedorOpcionDto> ObtenerMantenedorDeSesionAsync(
            CancellationToken cancellationToken = default);
    }
}
