using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public interface IInversorRepository
    {
        Task<int> AgregarAsync(Inversor inversor, CancellationToken cancellationToken = default);

        Task<Inversor?> ObtenerPorNodoAsync(int nodoId, CancellationToken cancellationToken = default);

        Task ActualizarAsync(Inversor inversor, CancellationToken cancellationToken = default);

        Task EliminarAsync(Inversor inversor, CancellationToken cancellationToken = default);
    }
}
