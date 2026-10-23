using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Usuario;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia
{
    public interface IUsuarioDbContext
    {
        DbSet<Usuario> Usuarios { get; set; }

        DbSet<Rol> Roles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
