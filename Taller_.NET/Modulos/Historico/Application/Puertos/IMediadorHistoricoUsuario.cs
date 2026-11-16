using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.Historico.Application.Dtos;

namespace Taller_.NET.Modulos.Historico.Application.Puertos
{
    public interface IMediadorHistoricoUsuario
    {
        Task<ResultadoMantenedor> ConfirmarMantenedorAsync(
            Guid idMantenedor,
            CancellationToken cancellationToken = default);

        Task<MantenedorOpcionDto?> ObtenerMantenedorPorAuth0Async(
            string? auth0UserId,
            CancellationToken cancellationToken = default);
    }
}
