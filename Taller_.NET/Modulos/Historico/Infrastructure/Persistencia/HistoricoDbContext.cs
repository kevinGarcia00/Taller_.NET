using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Infrastructure.Persistencia.Configuraciones;
using EntidadHistorico = Taller_.NET.Modulos.Historico.Domain.Historico;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia
{
    public class HistoricoDbContext : DbContext, IHistoricoDbContext
    {
        public HistoricoDbContext(DbContextOptions<HistoricoDbContext> options) : base(options)
        {
        }

        public DbSet<EntidadHistorico> Registros { get; set; } = null!;

        public DbSet<HistoricoMantenibilidad> HistoricosMantenibilidad { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HistoricoConfiguration());
            modelBuilder.ApplyConfiguration(new HistoricoMantenibilidadConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
