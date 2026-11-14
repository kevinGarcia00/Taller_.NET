using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Domain;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia
{
    public interface IHistoricoDbContext
    {
        DbSet<HistoricoMantenibilidad> Registros { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
