using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.Historico.Application.Dtos;

namespace Taller_.NET.Modulos.Historico.Application.Puertos
{
    public interface IMediadorHistoricoParqueSolar
    {
        Task<ContextoPanelLecturaDto?> ObtenerContextoAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default);
    }
}
