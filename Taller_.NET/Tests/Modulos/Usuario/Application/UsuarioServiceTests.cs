using FluentValidation;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Servicios;
using Xunit;
using EntidadUsuario = Taller_.NET.Modulos.Usuario.Usuario;

namespace Taller_.NET.Tests.Modulos.Usuario.Application;

public class UsuarioServiceTests
{
    [Fact]
    public async Task EliminarUsuarioAsync_ConUnicoAdministrador_LanzaExcepcion()
    {
        var administrador = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");
        administrador.AsignarRol(new Rol(1, Rol.NombreAdministrador));

        var repositorio = new FakeUsuarioRepository(administrador, 1);
        var proveedorActual = new FakeUsuarioActualProvider(true);
        var servicio = new UsuarioService(
            repositorio,
            proveedorActual,
            new InlineValidator<CrearUsuarioDto>(),
            new InlineValidator<ModificarUsuarioDto>());

        await Assert.ThrowsAsync<NoSePuedeEliminarUltimoAdministradorException>(
            () => servicio.EliminarUsuarioAsync(administrador.Id));

        Assert.False(repositorio.Eliminado);
    }

    private sealed class FakeUsuarioRepository : IUsuarioRepository
    {
        private readonly EntidadUsuario _usuario;
        private readonly int _cantidadAdministradores;

        public bool Eliminado { get; private set; }

        public FakeUsuarioRepository(EntidadUsuario usuario, int cantidadAdministradores)
        {
            _usuario = usuario;
            _cantidadAdministradores = cantidadAdministradores;
        }

        public Task<bool> ExisteUsuarioConCorreoAsync(string correo, CancellationToken cancellationToken = default) => Task.FromResult(false);

        public Task<EntidadUsuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(_usuario.Id == id ? _usuario : null);

        public Task<EntidadUsuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
            => Task.FromResult<EntidadUsuario?>(null);

        public Task<List<Rol>> ObtenerRolesPorIdsAsync(IReadOnlyCollection<long> rolesIds, CancellationToken cancellationToken = default)
            => Task.FromResult(_usuario.Roles.Where(rol => rolesIds.Contains(rol.Id)).ToList());

        public Task<List<Rol>> ObtenerTodosLosRolesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(new List<Rol>());

        public Task<List<UsuarioListadoDto>> ObtenerListadoAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(new List<UsuarioListadoDto>());

        public Task AgregarAsync(EntidadUsuario usuario, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task ActualizarAsync(EntidadUsuario usuario, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task Eliminar(EntidadUsuario usuario, CancellationToken cancellationToken = default)
        {
            Eliminado = true;
            return Task.CompletedTask;
        }

        public Task<int> ObtenerCantidadAdministradoresAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(_cantidadAdministradores);
    }

    private sealed class FakeUsuarioActualProvider : IUsuarioActualProvider
    {
        private readonly bool _esAdministrador;

        public FakeUsuarioActualProvider(bool esAdministrador)
        {
            _esAdministrador = esAdministrador;
        }

        public Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(_esAdministrador);
    }
}
