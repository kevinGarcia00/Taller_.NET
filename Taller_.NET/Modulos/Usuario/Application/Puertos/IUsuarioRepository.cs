using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;

namespace Taller_.NET.Modulos.Usuario.Application.Puertos
{
    public interface IUsuarioRepository
    {
        Task<bool> ExisteUsuarioConCorreoAsync(string correo, CancellationToken cancellationToken = default);

        Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);

        Task<Usuario?> ObtenerPorAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default);

        Task<List<UsuarioListadoDto>> ObtenerListadoAsync(CancellationToken cancellationToken = default);

        Task AgregarAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task ActualizarAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task EliminarAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task GuardarCambiosAsync(CancellationToken cancellationToken = default);
    }
}
