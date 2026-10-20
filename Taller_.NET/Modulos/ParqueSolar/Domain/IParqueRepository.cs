using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public interface IParqueRepository
    {
        Task<bool> ExisteParqueConNombreAsync(string nombre, CancellationToken cancellationToken = default);

        Task<int> AgregarAsync(Parque parque, CancellationToken cancellationToken = default);
    }
}
