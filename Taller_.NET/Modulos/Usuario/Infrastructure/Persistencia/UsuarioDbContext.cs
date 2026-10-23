using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia.Configuraciones;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia
{
    public class UsuarioDbContext : DbContext, IUsuarioDbContext
    {
        public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;

        public DbSet<Rol> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
