using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Puertos
{
    public interface IMediadorNodoParque
    {
        Task<bool> ExisteParqueAsync(int parqueSolarId, CancellationToken cancellationToken = default);
    }
}
