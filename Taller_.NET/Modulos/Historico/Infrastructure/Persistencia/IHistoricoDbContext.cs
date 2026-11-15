using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Domain;
using EntidadHistorico = Taller_.NET.Modulos.Historico.Domain.Historico;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia
{
    public interface IHistoricoDbContext
    {
        DbSet<EntidadHistorico> Registros { get; set; }

        DbSet<HistoricoMantenibilidad> HistoricosMantenibilidad { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
