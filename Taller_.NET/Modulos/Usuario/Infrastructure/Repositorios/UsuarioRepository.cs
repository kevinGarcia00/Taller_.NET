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
                .FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.Correo == correo, cancellationToken);
        }

        public async Task<Usuario?> ObtenerPorAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.Auth0UserId == auth0UserId, cancellationToken);
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
                    Auth0UserId = usuario.Auth0UserId
                })
                .ToListAsync(cancellationToken);
        }

        public async Task AgregarAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            await _contexto.Usuarios.AddAsync(usuario, cancellationToken);
        }

        public Task ActualizarAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            _contexto.Usuarios.Update(usuario);

            return Task.CompletedTask;
        }

        public Task EliminarAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            _contexto.Usuarios.Remove(usuario);

            return Task.CompletedTask;
        }

        public async Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
        {
            await _contexto.SaveChangesAsync(cancellationToken);
        }
    }
}
