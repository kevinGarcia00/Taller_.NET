using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;
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
        administrador.VincularConAuth0("auth0|administrador");

        var repositorio = new FakeUsuarioRepository(administrador);
        var proveedorActual = new FakeUsuarioActualProvider(tienePermisos: true);
        var auth0 = new FakeAuth0AdministracionProvider(administrador.Auth0UserId!, RolesAuth0.Administrador, 1);
        var servicio = new UsuarioService(
            repositorio,
            proveedorActual,
            auth0,
            new InlineValidator<CrearUsuarioDto>(),
            new InlineValidator<ModificarUsuarioDto>(),
            NullLogger<UsuarioService>.Instance);

        await Assert.ThrowsAsync<NoSePuedeEliminarUltimoAdministradorException>(
            () => servicio.EliminarUsuarioAsync(administrador.Id));

        Assert.False(repositorio.Eliminado);
    }

    private sealed class FakeUsuarioRepository : IUsuarioRepository
    {
        private readonly EntidadUsuario _usuario;

        public bool Eliminado { get; private set; }

        public FakeUsuarioRepository(EntidadUsuario usuario)
        {
            _usuario = usuario;
        }

        public Task<bool> ExisteUsuarioConCorreoAsync(string correo, CancellationToken cancellationToken = default)
            => Task.FromResult(false);

        public Task<EntidadUsuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(_usuario.Id == id ? _usuario : null);

        public Task<EntidadUsuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
            => Task.FromResult<EntidadUsuario?>(null);

        public Task<EntidadUsuario?> ObtenerPorAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
            => Task.FromResult(_usuario.Auth0UserId == auth0UserId ? _usuario : null);

        public Task<List<UsuarioListadoDto>> ObtenerListadoAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(new List<UsuarioListadoDto>());

        public Task AgregarAsync(EntidadUsuario usuario, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task ActualizarAsync(EntidadUsuario usuario, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task EliminarAsync(EntidadUsuario usuario, CancellationToken cancellationToken = default)
        {
            Eliminado = true;
            return Task.CompletedTask;
        }

        public Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }

    private sealed class FakeUsuarioActualProvider : IUsuarioActualProvider
    {
        private readonly bool _tienePermisos;

        public FakeUsuarioActualProvider(bool tienePermisos)
        {
            _tienePermisos = tienePermisos;
        }

        public Task<bool> TienePermisoAsync(string permiso, CancellationToken cancellationToken = default)
            => Task.FromResult(_tienePermisos);

        public Task<string?> ObtenerAuth0UserIdAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<string?>(null);

        public Task<string?> ObtenerCorreoAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<string?>(null);
    }

    private sealed class FakeAuth0AdministracionProvider : IAuth0AdministracionProvider
    {
        private const string RolAdministradorId = "rol|administrador";

        private readonly string _auth0UserId;
        private readonly string _nombreRol;
        private readonly int _cantidadUsuariosConRol;

        public FakeAuth0AdministracionProvider(
            string auth0UserId,
            string nombreRol,
            int cantidadUsuariosConRol)
        {
            _auth0UserId = auth0UserId;
            _nombreRol = nombreRol;
            _cantidadUsuariosConRol = cantidadUsuariosConRol;
        }

        public Task<string> CrearUsuarioAsync(
            string correo,
            string contrasena,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default)
            => Task.FromResult($"auth0|{Guid.NewGuid():N}");

        public Task ActualizarUsuarioAsync(
            string auth0UserId,
            string correo,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task EliminarUsuarioAsync(string auth0UserId, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(new List<RolDto>
            {
                new RolDto { Id = RolAdministradorId, Nombre = RolesAuth0.Administrador }
            });

        public Task<List<RolDto>> ObtenerRolesDeUsuarioAsync(
            string auth0UserId,
            CancellationToken cancellationToken = default)
        {
            if (auth0UserId != _auth0UserId)
            {
                return Task.FromResult(new List<RolDto>());
            }

            return Task.FromResult(new List<RolDto>
            {
                new RolDto { Id = RolAdministradorId, Nombre = _nombreRol }
            });
        }

        public Task ReemplazarRolesAsync(
            string auth0UserId,
            IReadOnlyCollection<string> rolesIds,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task<int> ObtenerCantidadUsuariosConRolAsync(
            string nombreRol,
            CancellationToken cancellationToken = default)
            => Task.FromResult(_cantidadUsuariosConRol);
    }
}
