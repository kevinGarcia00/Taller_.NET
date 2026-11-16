using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Excepciones;
using Taller_.NET.Modulos.Historico.Application.Puertos;
using Taller_.NET.Modulos.Historico.Domain;

namespace Taller_.NET.Modulos.Historico.Application.Servicios
{
    public class HistoricoMantenibilidadService : IHistoricoMantenibilidadService
    {
        private readonly IHistoricoMantenibilidadRepository _repositorio;
        private readonly IMediadorHistoricoParqueSolar _mediadorParqueSolar;
        private readonly IMediadorHistoricoUsuario _mediadorUsuario;
        private readonly IUsuarioActualHistorico _usuarioActual;
        private readonly IValidator<CrearHistoricoMantenibilidadDto> _validadorCrear;

        public HistoricoMantenibilidadService(
            IHistoricoMantenibilidadRepository repositorio,
            IMediadorHistoricoParqueSolar mediadorParqueSolar,
            IMediadorHistoricoUsuario mediadorUsuario,
            IUsuarioActualHistorico usuarioActual,
            IValidator<CrearHistoricoMantenibilidadDto> validadorCrear)
        {
            _repositorio = repositorio;
            _mediadorParqueSolar = mediadorParqueSolar;
            _mediadorUsuario = mediadorUsuario;
            _usuarioActual = usuarioActual;
            _validadorCrear = validadorCrear;
        }

        public async Task<int> RegistrarRegistroAsync(
            CrearHistoricoMantenibilidadDto dto,
            CancellationToken cancellationToken = default)
        {
            var mantenedorDeSesion = await ObtenerMantenedorDeSesionAsync(cancellationToken);
            dto.IdMantenedor = mantenedorDeSesion.Id;

            await ValidarDtoAsync(dto, cancellationToken);

            var contexto = await ObtenerContextoObligatorioAsync(
                dto.PanelId,
                dto.NodoId,
                dto.ParqueSolarId,
                cancellationToken);

            await AsegurarMantenedorValidoAsync(dto.IdMantenedor, cancellationToken);

            var registro = new HistoricoMantenibilidad(
                new Mantenedor(dto.IdMantenedor),
                dto.Fecha,
                dto.Observaciones,
                contexto.Estado,
                dto.EstadoResultante,
                new ContextoPanel(dto.PanelId, dto.NodoId, dto.ParqueSolarId));

            return await _repositorio.AgregarAsync(registro, cancellationToken);
        }

        public async Task<IReadOnlyList<HistoricoMantenibilidadListadoDto>> ObtenerRegistrosPorPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            if (!await _usuarioActual.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException(
                    "Solo un administrador puede ver el historial de mantenibilidad.");
            }

            await ObtenerContextoObligatorioAsync(panelId, nodoId, parqueSolarId, cancellationToken);

            var registros = await _repositorio.ObtenerPorPanelAsync(panelId, cancellationToken);

            return registros
                .Select(registro => new HistoricoMantenibilidadListadoDto
                {
                    Id = registro.Id,
                    IdMantenedor = registro.Mantenedor.IdMantenedor,
                    Fecha = registro.Fecha,
                    Observaciones = registro.Observaciones,
                    EstadoInicial = registro.EstadoInicial.ToString(),
                    EstadoResultante = registro.EstadoResultante.ToString()
                })
                .ToList();
        }

        public async Task<ContextoPanelLecturaDto> ObtenerContextoPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            return await ObtenerContextoObligatorioAsync(panelId, nodoId, parqueSolarId, cancellationToken);
        }

        public async Task<MantenedorOpcionDto> ObtenerMantenedorDeSesionAsync(
            CancellationToken cancellationToken = default)
        {
            if (!await _usuarioActual.EsMantenimientoAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException(
                    "Solo un usuario de Mantenimiento puede registrar un mantenimiento.");
            }

            var auth0UserId = await _usuarioActual.ObtenerAuth0UserIdAsync(cancellationToken);
            var mantenedor = await _mediadorUsuario.ObtenerMantenedorPorAuth0Async(
                auth0UserId,
                cancellationToken);

            if (mantenedor is null)
            {
                throw new MantenedorNoEncontradoException(
                    "El usuario que inicio sesion no esta vinculado a un usuario de la aplicacion.");
            }

            return mantenedor;
        }

        private async Task<ContextoPanelLecturaDto> ObtenerContextoObligatorioAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken)
        {
            var contexto = await _mediadorParqueSolar.ObtenerContextoAsync(
                panelId,
                nodoId,
                parqueSolarId,
                cancellationToken);

            if (contexto is null)
            {
                throw new PanelFueraDeContextoException(panelId);
            }

            return contexto;
        }

        private async Task AsegurarMantenedorValidoAsync(Guid idMantenedor, CancellationToken cancellationToken)
        {
            var resultado = await _mediadorUsuario.ConfirmarMantenedorAsync(idMantenedor, cancellationToken);

            if (resultado == ResultadoMantenedor.Inexistente)
            {
                throw new MantenedorNoEncontradoException();
            }

            if (resultado == ResultadoMantenedor.SinRol)
            {
                throw new MantenedorSinRolException();
            }
        }

        private async Task ValidarDtoAsync(
            CrearHistoricoMantenibilidadDto dto,
            CancellationToken cancellationToken)
        {
            var resultado = await _validadorCrear.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }
    }
}
