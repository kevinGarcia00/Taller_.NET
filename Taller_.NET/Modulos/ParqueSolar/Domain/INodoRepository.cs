using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public interface INodoRepository
    {
        Task<bool> ExisteNodoConNombreAsync(
            string nombre,
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<bool> ExisteInversorEnUsoAsync(
            int inversorId,
            CancellationToken cancellationToken = default);

        Task<bool> ExisteInversorEnOtroNodoAsync(
            int inversorId,
            int nodoIdActual,
            CancellationToken cancellationToken = default);

        Task<bool> ExisteOtroNodoConNombreAsync(
            string nombre,
            int parqueSolarId,
            int nodoIdActual,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Nodo>> ObtenerPorParqueAsync(
            int parqueSolarId,
            CancellationToken cancellationToken = default);

        Task<Nodo?> ObtenerPorIdAsync(
            int nodoId,
            CancellationToken cancellationToken = default);

        Task<int> AgregarAsync(Nodo nodo, CancellationToken cancellationToken = default);

        Task ActualizarAsync(Nodo nodo, CancellationToken cancellationToken = default);

        Task EliminarAsync(Nodo nodo, CancellationToken cancellationToken = default);
    }
}
