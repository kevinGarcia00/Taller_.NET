using Application.Dtos;
using Application.Puertos;

namespace Application.Servicios;

public sealed class UsuarioConsultaService : IUsuarioConsultaService
{
    private readonly IUsuarioConsultaRepository _repositorioUsuarios;

    public UsuarioConsultaService(IUsuarioConsultaRepository repositorioUsuarios)
    {
        _repositorioUsuarios = repositorioUsuarios;
    }

    public Task<IReadOnlyList<UsuarioListadoDto>> ObtenerUsuariosAsync(
        CancellationToken cancellationToken = default)
    {
        return _repositorioUsuarios.ObtenerUsuariosAsync(cancellationToken);
    }

}
