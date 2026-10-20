using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Puertos
{
    public interface IUsuarioActualProvider
    {
        Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default);
    }
}
