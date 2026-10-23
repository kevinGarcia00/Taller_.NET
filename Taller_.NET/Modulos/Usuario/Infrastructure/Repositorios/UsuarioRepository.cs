using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IUsuarioDbContext _contexto;

        public UsuarioRepository(IUsuarioDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> ExisteUsuarioConCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _contexto.Usuarios
                .AnyAsync(usuario => usuario.Correo == correo, cancellationToken);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _contexto.Usuarios
                .Include(usuario => usuario.Roles)
                .FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _contexto.Usuarios
                .Include(usuario => usuario.Roles)
                .FirstOrDefaultAsync(usuario => usuario.Correo == correo, cancellationToken);
        }

        public async Task<List<Rol>> ObtenerRolesPorIdsAsync(IReadOnlyCollection<long> rolesIds, CancellationToken cancellationToken = default)
        {
            return await _contexto.Roles
                .Where(rol => rolesIds.Contains(rol.Id))
                .OrderBy(rol => rol.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Rol>> ObtenerTodosLosRolesAsync(CancellationToken cancellationToken = default)
        {
            return await _contexto.Roles
                .OrderBy(rol => rol.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UsuarioListadoDto>> ObtenerListadoAsync(CancellationToken cancellationToken = default)
        {
            return await _contexto.Usuarios
                .OrderBy(usuario => usuario.Apellido)
                .ThenBy(usuario => usuario.Nombre)
                .Select(usuario => new UsuarioListadoDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Correo = usuario.Correo,
                    Telefono = usuario.Telefono,
                    Roles = usuario.Roles.Select(rol => rol.Nombre).ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task AgregarAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            await _contexto.Usuarios.AddAsync(usuario, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task ActualizarAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            _contexto.Usuarios.Update(usuario);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task Eliminar(Usuario usuario, CancellationToken cancellationToken = default)
        {
            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> ObtenerCantidadAdministradoresAsync(CancellationToken cancellationToken = default)
        {
            return await _contexto.Usuarios
                .CountAsync(usuario => usuario.Roles.Any(rol => rol.Nombre == Taller_.NET.Modulos.Usuario.Rol.NombreAdministrador), cancellationToken);
        }
    }
}
