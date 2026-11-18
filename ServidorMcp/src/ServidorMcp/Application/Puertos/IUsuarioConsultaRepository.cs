using Application.Dtos;

namespace Application.Puertos;

public interface IUsuarioConsultaRepository
{
    Task<IReadOnlyList<UsuarioListadoDto>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default);
}
