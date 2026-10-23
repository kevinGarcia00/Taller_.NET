using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;

namespace Taller_.NET.Modulos.Usuario.Application
{
    public interface IUsuarioService
    {
        Task<Guid> CrearUsuarioAsync(CrearUsuarioDto dto, CancellationToken cancellationToken = default);

        Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(Guid id, CancellationToken cancellationToken = default);

        Task ModificarUsuarioAsync(ModificarUsuarioDto dto, CancellationToken cancellationToken = default);

        Task EliminarUsuarioAsync(Guid id, Guid idUsuarioAutenticado = default, CancellationToken cancellationToken = default);

        Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default);

        Task<List<UsuarioListadoDto>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default);
    }
}
