using System.Collections.Concurrent;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Auth0
{
    /// <summary>
    /// Implementacion usada cuando la aplicacion corre en modo anonimo de desarrollo
    /// (Auth0:PermitirUsuariosAnonimos=true). Permite usar todas las funciones del
    /// modulo sin credenciales de la Management API, manteniendo los usuarios en la
    /// base local y los roles simulados en memoria.
    /// </summary>
    public class Auth0AdministracionProviderAnonimo : IAuth0AdministracionProvider
    {
        private const string PrefijoIdentificador = "auth0|anonimo-";
        private const string RolAdministradorId = "anonimo|administrador";
        private const string RolMantenimientoId = "anonimo|mantenimiento";

        private static readonly ConcurrentDictionary<string, List<string>> RolesPorUsuario = new();

        private static readonly List<RolDto> Roles = new()
        {
            new RolDto { Id = RolAdministradorId, Nombre = RolesAuth0.Administrador },
            new RolDto { Id = RolMantenimientoId, Nombre = RolesAuth0.Mantenimiento }
        };

        private readonly ILogger<Auth0AdministracionProviderAnonimo> _logger;

        public Auth0AdministracionProviderAnonimo(ILogger<Auth0AdministracionProviderAnonimo> logger)
        {
            _logger = logger;
        }

        public Task<string> CrearUsuarioAsync(
            string correo,
            string contrasena,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default)
        {
            var auth0UserId = $"{PrefijoIdentificador}{Guid.NewGuid():N}";

            RolesPorUsuario[auth0UserId] = new List<string>();

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Modo anonimo de desarrollo: se genero el identificador local {Auth0UserId} para {Correo}. " +
                    "No se creo ninguna cuenta en Auth0.",
                    auth0UserId,
                    correo);
            }

            return Task.FromResult(auth0UserId);
        }

        public Task ActualizarUsuarioAsync(
            string auth0UserId,
            string correo,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Modo anonimo de desarrollo: se omite la actualizacion en Auth0 del usuario {Auth0UserId}.",
                    auth0UserId);
            }

            return Task.CompletedTask;
        }

        public Task EliminarUsuarioAsync(string auth0UserId, CancellationToken cancellationToken = default)
        {
            RolesPorUsuario.TryRemove(auth0UserId, out _);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Modo anonimo de desarrollo: se omite la eliminacion en Auth0 del usuario {Auth0UserId}.",
                    auth0UserId);
            }

            return Task.CompletedTask;
        }

        public Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Roles.ToList());
        }

        public Task<IReadOnlyDictionary<string, List<string>>> ObtenerNombresDeRolesPorUsuarioAsync(
            CancellationToken cancellationToken = default)
        {
            var rolesPorUsuario = new Dictionary<string, List<string>>(StringComparer.Ordinal);

            foreach (var (auth0UserId, rolesIds) in RolesPorUsuario)
            {
                rolesPorUsuario[auth0UserId] = Roles
                    .Where(rol => rolesIds.Contains(rol.Id))
                    .Select(rol => rol.Nombre)
                    .ToList();
            }

            return Task.FromResult<IReadOnlyDictionary<string, List<string>>>(rolesPorUsuario);
        }

        public Task<List<RolDto>> ObtenerRolesDeUsuarioAsync(
            string auth0UserId,
            CancellationToken cancellationToken = default)
        {
            if (!RolesPorUsuario.TryGetValue(auth0UserId, out var rolesIds))
            {
                return Task.FromResult(new List<RolDto>());
            }

            var roles = Roles.Where(rol => rolesIds.Contains(rol.Id)).ToList();

            return Task.FromResult(roles);
        }

        public Task ReemplazarRolesAsync(
            string auth0UserId,
            IReadOnlyCollection<string> rolesIds,
            CancellationToken cancellationToken = default)
        {
            RolesPorUsuario[auth0UserId] = rolesIds.Distinct().ToList();

            return Task.CompletedTask;
        }

        public Task<int> ObtenerCantidadUsuariosConRolAsync(
            string nombreRol,
            CancellationToken cancellationToken = default)
        {
            var rol = Roles.FirstOrDefault(rol =>
                string.Equals(rol.Nombre, nombreRol, StringComparison.OrdinalIgnoreCase));

            if (rol is null)
            {
                return Task.FromResult(0);
            }

            var cantidad = RolesPorUsuario.Values.Count(rolesIds => rolesIds.Contains(rol.Id));

            return Task.FromResult(cantidad);
        }
    }
}
