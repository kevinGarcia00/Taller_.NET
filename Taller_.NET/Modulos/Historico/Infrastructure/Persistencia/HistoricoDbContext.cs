using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Infrastructure.Persistencia.Configuraciones;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia
{
    public class HistoricoDbContext : DbContext, IHistoricoDbContext
    {
        public HistoricoDbContext(DbContextOptions<HistoricoDbContext> options)
            : base(options)
        {
        }

        public DbSet<HistoricoMantenibilidad> Registros { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HistoricoMantenibilidadConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
