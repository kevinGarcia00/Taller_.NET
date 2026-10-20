using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia
{
    public interface IParqueDbContext
    {
        DbSet<Parque> Parques { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
