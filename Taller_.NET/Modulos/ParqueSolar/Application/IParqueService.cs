using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application
{
    public interface IParqueService
    {
        Task<int> CrearParqueAsync(CrearParqueDto dto, CancellationToken cancellationToken = default);

        Task<int> ModificarParqueAsync(int parqueSolarId, ModificarParqueDto dto, CancellationToken cancellationToken = default);

        Task EliminarParqueAsync(int parqueSolarId, CancellationToken cancellationToken = default);

        IReadOnlyList<ParqueListadoDto> ObtenerParquesSync();

        ParqueDetalleDto ObtenerParqueSync(int parqueSolarId);
    }
}
