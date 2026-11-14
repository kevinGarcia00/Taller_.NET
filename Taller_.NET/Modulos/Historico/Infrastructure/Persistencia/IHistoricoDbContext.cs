using Microsoft.EntityFrameworkCore;
using EntidadHistorico = Taller_.NET.Modulos.Historico.Domain.Historico;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia
{
    public interface IHistoricoDbContext
    {
        DbSet<EntidadHistorico> Registros { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
