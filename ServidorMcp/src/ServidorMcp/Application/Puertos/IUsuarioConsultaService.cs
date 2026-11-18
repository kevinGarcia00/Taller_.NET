using Application.Dtos;

namespace Application.Puertos;

public interface IUsuarioConsultaService
{
    Task<IReadOnlyList<UsuarioListadoDto>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default);
}
