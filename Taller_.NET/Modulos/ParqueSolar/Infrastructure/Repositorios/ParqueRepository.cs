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

        public async Task<int> AgregarAsync(Parque parque, CancellationToken cancellationToken = default)
        {
            await _contexto.Parques.AddAsync(parque, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            return parque.Id;
        }
    }
}
