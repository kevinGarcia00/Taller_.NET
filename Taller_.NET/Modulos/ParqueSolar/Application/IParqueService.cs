using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface IParqueService
    {
        Task<int> CrearParqueAsync(CrearParqueDto dto, CancellationToken cancellationToken = default);
    }
}
