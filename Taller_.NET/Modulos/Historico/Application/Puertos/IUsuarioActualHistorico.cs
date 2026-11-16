using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.Historico.Application.Puertos
{
    public interface IUsuarioActualHistorico
    {
        Task<bool> EsMantenimientoAsync(CancellationToken cancellationToken = default);

        Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default);

        Task<string?> ObtenerAuth0UserIdAsync(CancellationToken cancellationToken = default);
    }
}
