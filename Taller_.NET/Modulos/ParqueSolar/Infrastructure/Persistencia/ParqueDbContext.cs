using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Configuraciones;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia
{
    public class ParqueDbContext : DbContext, IParqueDbContext
    {
        public ParqueDbContext(DbContextOptions<ParqueDbContext> options) : base(options)
        {
        }

        public DbSet<Parque> Parques { get; set; } = null!;

        public DbSet<NodoDomain> Nodos { get; set; } = null!;

        public DbSet<InversorDomain> Inversores { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ParqueConfiguration());
            modelBuilder.ApplyConfiguration(new NodoConfiguration());
            modelBuilder.ApplyConfiguration(new InversorConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
