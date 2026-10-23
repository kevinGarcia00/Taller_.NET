using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;

namespace Taller_.NET.Modulos.Usuario.Application.Puertos
{
    public interface IUsuarioRepository
    {
        Task<bool> ExisteUsuarioConCorreoAsync(string correo, CancellationToken cancellationToken = default);

        Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);

        Task<List<Rol>> ObtenerRolesPorIdsAsync(IReadOnlyCollection<long> rolesIds, CancellationToken cancellationToken = default);

        Task<List<Rol>> ObtenerTodosLosRolesAsync(CancellationToken cancellationToken = default);

        Task<List<UsuarioListadoDto>> ObtenerListadoAsync(CancellationToken cancellationToken = default);

        Task AgregarAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task ActualizarAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task Eliminar(Usuario usuario, CancellationToken cancellationToken = default);

        Task<int> ObtenerCantidadAdministradoresAsync(CancellationToken cancellationToken = default);
    }
}
