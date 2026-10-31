using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Servicios
{
    public class PanelService : IPanelService
    {
        private readonly IPanelRepository _panelRepository;
        private readonly INodoRepository _nodoRepository;
        private readonly IUsuarioActualProvider _usuarioActualProvider;
        private readonly IValidator<CrearPanelDto> _validadorCrear;
        private readonly IValidator<ModificarPanelDto> _validadorModificar;

        public PanelService(
            IPanelRepository panelRepository,
            INodoRepository nodoRepository,
            IUsuarioActualProvider usuarioActualProvider,
            IValidator<CrearPanelDto> validadorCrear,
            IValidator<ModificarPanelDto> validadorModificar)
        {
            _panelRepository = panelRepository;
            _nodoRepository = nodoRepository;
            _usuarioActualProvider = usuarioActualProvider;
            _validadorCrear = validadorCrear;
            _validadorModificar = validadorModificar;
        }

        public async Task<int> CrearPanelAsync(CrearPanelDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(dto, cancellationToken);
            await AsegurarAdministradorAsync("Solo un administrador puede registrar paneles.", cancellationToken);
            var nodo = await ObtenerNodoDelParqueAsync(dto.NodoId, dto.ParqueSolarId, cancellationToken);

            var nombreNormalizado = dto.Nombre.Trim();

            if (await _panelRepository.ExistePanelConNombreAsync(nombreNormalizado, dto.NodoId, cancellationToken))
            {
                throw new PanelNombreDuplicadoException(nombreNormalizado);
            }

            var panel = new PanelDomain(dto.Nombre, dto.Estado, dto.Potencia, dto.VidaUtil);
            panel.AsignarNodo(nodo);

            return await _panelRepository.AgregarAsync(panel, cancellationToken);
        }

        public async Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesPorNodoAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            await AsegurarAdministradorAsync("Solo un administrador puede ver los paneles.", cancellationToken);
            await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);

            var paneles = await _panelRepository.ObtenerPorNodoAsync(nodoId, cancellationToken);

            return paneles
                .Select(panel => new PanelListadoDto
                {
                    Id = panel.Id,
                    Nombre = panel.Nombre,
                    Estado = panel.Estado.ToString(),
                    Potencia = panel.Potencia
                })
                .ToList();
        }

        public async Task<PanelDetalleDto> ObtenerPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            await AsegurarAdministradorAsync("Solo un administrador puede ver los paneles.", cancellationToken);
            var nodo = await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);
            var panel = await _panelRepository.ObtenerPorIdYNodoAsync(panelId, nodoId, cancellationToken);

            if (panel is null)
            {
                throw new PanelNoEncontradoException(panelId);
            }

            return CrearDetalle(panel, nodo.ParqueSolarId);
        }

        public async Task EliminarPanelAsync(
            int id,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            await AsegurarAdministradorAsync("Solo un administrador puede eliminar paneles.", cancellationToken);
            await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);

            var panel = await _panelRepository.ObtenerPorIdYNodoAsync(id, nodoId, cancellationToken);

            if (panel is null)
            {
                throw new PanelNoEncontradoException(id);
            }

            await _panelRepository.EliminarAsync(panel, cancellationToken);
        }

        public async Task<ModificarPanelDto> ObtenerParaEdicionAsync(
            int id,
            int nodoId,
            CancellationToken cancellationToken = default)
        {
            await AsegurarAdministradorAsync("Solo un administrador puede modificar paneles.", cancellationToken);

            var panel = await _panelRepository.ObtenerPorIdYNodoAsync(id, nodoId, cancellationToken);

            if (panel is null)
            {
                throw new PanelNoEncontradoException(id);
            }

            var nodo = await ObtenerNodoRegistradoAsync(nodoId, cancellationToken);

            return new ModificarPanelDto
            {
                Id = panel.Id,
                Nombre = panel.Nombre,
                Estado = panel.Estado,
                Potencia = panel.Potencia,
                VidaUtil = panel.VidaUtil,
                FechaCreacion = panel.FechaCreacion,
                NodoId = panel.NodoId,
                ParqueSolarId = nodo.ParqueSolarId
            };
        }

        public async Task ModificarPanelAsync(ModificarPanelDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(dto, cancellationToken);
            await AsegurarAdministradorAsync("Solo un administrador puede modificar paneles.", cancellationToken);
            await ObtenerNodoDelParqueAsync(dto.NodoId, dto.ParqueSolarId, cancellationToken);

            var panel = await _panelRepository.ObtenerPorIdYNodoAsync(dto.Id, dto.NodoId, cancellationToken);

            if (panel is null)
            {
                throw new PanelNoEncontradoException(dto.Id);
            }

            var nombreNormalizado = dto.Nombre.Trim();

            if (await _panelRepository.ExisteOtroPanelConNombreAsync(nombreNormalizado, dto.NodoId, dto.Id, cancellationToken))
            {
                throw new PanelNombreDuplicadoException(nombreNormalizado);
            }

            panel.ActualizarDatos(dto.Nombre, dto.Estado, dto.Potencia, dto.VidaUtil);
            await _panelRepository.ActualizarAsync(panel, cancellationToken);
        }

        private static PanelDetalleDto CrearDetalle(PanelDomain panel, int parqueSolarId)
        {
            return new PanelDetalleDto
            {
                Id = panel.Id,
                Nombre = panel.Nombre,
                Estado = panel.Estado.ToString(),
                Potencia = panel.Potencia,
                VidaUtil = panel.VidaUtil,
                FechaCreacion = panel.FechaCreacion,
                NodoId = panel.NodoId,
                ParqueSolarId = parqueSolarId
            };
        }

        private async Task AsegurarAdministradorAsync(string mensaje, CancellationToken cancellationToken)
        {
            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException(mensaje);
            }
        }

        private async Task<NodoDomain> ObtenerNodoRegistradoAsync(int nodoId, CancellationToken cancellationToken)
        {
            var nodo = await _nodoRepository.ObtenerPorIdAsync(nodoId, cancellationToken);

            if (nodo is null)
            {
                throw new NodoNoEncontradoException(nodoId);
            }

            return nodo;
        }

        private async Task<NodoDomain> ObtenerNodoDelParqueAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken)
        {
            var nodo = await _nodoRepository.ObtenerPorIdAsync(nodoId, cancellationToken);

            if (nodo is null || nodo.ParqueSolarId != parqueSolarId)
            {
                throw new NodoNoEncontradoException(nodoId);
            }

            return nodo;
        }

        private async Task ValidarDtoAsync(CrearPanelDto dto, CancellationToken cancellationToken)
        {
            var resultado = await _validadorCrear.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }

        private async Task ValidarDtoAsync(ModificarPanelDto dto, CancellationToken cancellationToken)
        {
            var resultado = await _validadorModificar.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }
    }
}
