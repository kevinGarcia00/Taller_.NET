using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios
{
    public class ParqueRepository : IParqueRepository
    {
        private readonly IParqueDbContext _contexto;

        public ParqueRepository(IParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> ExisteParqueConNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            return await _contexto.Parques
                .AnyAsync(parque => parque.Nombre == nombre, cancellationToken);
        }

        public async Task<bool> ExisteOtroParqueConNombreAsync(string nombre, int parqueId, CancellationToken cancellationToken = default)
        {
            return await _contexto.Parques
                .AnyAsync(parque => parque.Id != parqueId && parque.Nombre == nombre, cancellationToken);
        }

        public async Task<int> AgregarAsync(Parque parque, CancellationToken cancellationToken = default)
        {
            await _contexto.Parques.AddAsync(parque, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            return parque.Id;
        }

        public async Task ActualizarAsync(Parque parque, CancellationToken cancellationToken = default)
        {
            _contexto.Parques.Update(parque);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task EliminarAsync(Parque parque, CancellationToken cancellationToken = default)
        {
            _contexto.Parques.Remove(parque);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public Parque? ObtenerPorId(int parqueSolarId)
        {
            return _contexto.Parques
                .FirstOrDefault(parque => parque.Id == parqueSolarId);
        }

        public IReadOnlyList<Parque> ObtenerTodos()
        {
            return _contexto.Parques
                .OrderBy(parque => parque.Id)
                .ToList();
        }
    }
}
