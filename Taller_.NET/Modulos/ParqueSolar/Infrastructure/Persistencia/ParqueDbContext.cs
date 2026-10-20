using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia
{
    public class ParqueDbContext : DbContext, IParqueDbContext
    {
        public ParqueDbContext(DbContextOptions<ParqueDbContext> options) : base(options)
        {
        }

        public DbSet<Parque> Parques { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParqueDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
