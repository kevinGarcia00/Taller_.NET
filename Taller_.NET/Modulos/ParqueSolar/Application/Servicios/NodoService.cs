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

namespace Taller_.NET.Modulos.ParqueSolar.Application.Servicios
{
    public class NodoService : INodoService
    {
        private readonly INodoRepository _nodoRepository;
        private readonly IMediadorNodoParque _mediadorNodoParque;
        private readonly IMediadorNodoInversor _mediadorNodoInversor;
        private readonly IUsuarioActualProvider _usuarioActualProvider;
        private readonly IValidator<CrearNodoDto> _validadorCrear;
        private readonly IValidator<EditarNodoDto> _validadorEditar;

        public NodoService(
            INodoRepository nodoRepository,
            IMediadorNodoParque mediadorNodoParque,
            IMediadorNodoInversor mediadorNodoInversor,
            IUsuarioActualProvider usuarioActualProvider,
            IValidator<CrearNodoDto> validadorCrear,
            IValidator<EditarNodoDto> validadorEditar)
        {
            _nodoRepository = nodoRepository;
            _mediadorNodoParque = mediadorNodoParque;
            _mediadorNodoInversor = mediadorNodoInversor;
            _usuarioActualProvider = usuarioActualProvider;
            _validadorCrear = validadorCrear;
            _validadorEditar = validadorEditar;
        }

        public async Task<int> CrearNodoAsync(CrearNodoDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(dto, cancellationToken);

            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException("Solo un administrador puede registrar nodos.");
            }

            if (!await _mediadorNodoParque.ExisteParqueAsync(dto.ParqueSolarId, cancellationToken))
            {
                throw new ParqueNoEncontradoException(dto.ParqueSolarId);
            }

            if (dto.InversorId.HasValue && !await _mediadorNodoInversor.ExisteInversorAsync(dto.InversorId.Value, cancellationToken))
            {
                throw new InversorNoEncontradoException(dto.InversorId.Value);
            }

            if (dto.InversorId.HasValue && await _nodoRepository.ExisteInversorEnUsoAsync(dto.InversorId.Value, cancellationToken))
            {
                throw new InversorYaAsignadoException(dto.InversorId.Value);
            }

            var nombreNormalizado = dto.Nombre.Trim();

            if (await _nodoRepository.ExisteNodoConNombreAsync(nombreNormalizado, dto.ParqueSolarId, cancellationToken))
            {
                throw new NodoNombreDuplicadoException(nombreNormalizado);
            }

            var nodo = new NodoDomain(dto.Nombre, dto.UbicacionParque, dto.InversorId);
            nodo.AsignarContexto(new ContextoParque(dto.ParqueSolarId));

            return await _nodoRepository.AgregarAsync(nodo, cancellationToken);
        }

        public async Task<EditarNodoDto> ObtenerNodoParaEdicionAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            var nodo = await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);

            return new EditarNodoDto
            {
                Id = nodo.Id,
                Nombre = nodo.Nombre,
                UbicacionParque = nodo.UbicacionParque,
                InversorId = nodo.InversorId,
                ParqueSolarId = nodo.ParqueSolarId
            };
        }

        public async Task<IReadOnlyList<InversorOpcionDto>> ObtenerInversoresParaEdicionAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            var nodo = await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);

            return await _mediadorNodoInversor.ObtenerOpcionesParaNodoAsync(nodo.InversorId, cancellationToken);
        }

        public async Task ModificarNodoAsync(EditarNodoDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(dto, cancellationToken);

            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException("Solo un administrador puede modificar nodos.");
            }

            var nodo = await ObtenerNodoDelParqueAsync(dto.Id, dto.ParqueSolarId, cancellationToken);

            if (dto.InversorId.HasValue)
            {
                var inversorId = dto.InversorId.Value;

                if (!await _mediadorNodoInversor.ExisteInversorAsync(inversorId, cancellationToken))
                {
                    throw new InversorNoEncontradoException(inversorId);
                }

                if (await _nodoRepository.ExisteInversorEnOtroNodoAsync(inversorId, dto.Id, cancellationToken))
                {
                    throw new InversorYaAsignadoException(inversorId);
                }
            }

            var nombreNormalizado = dto.Nombre.Trim();

            if (await _nodoRepository.ExisteOtroNodoConNombreAsync(
                    nombreNormalizado,
                    dto.ParqueSolarId,
                    dto.Id,
                    cancellationToken))
            {
                throw new NodoNombreDuplicadoException(nombreNormalizado);
            }

            nodo.Actualizar(dto.Nombre, dto.UbicacionParque, dto.InversorId);

            await _nodoRepository.ActualizarAsync(nodo, cancellationToken);
        }

        public async Task<IReadOnlyList<NodoListadoDto>> ObtenerNodosPorParqueAsync(
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            if (!await _mediadorNodoParque.ExisteParqueAsync(parqueSolarId, cancellationToken))
            {
                throw new ParqueNoEncontradoException(parqueSolarId);
            }

            var nodos = await _nodoRepository.ObtenerPorParqueAsync(parqueSolarId, cancellationToken);
            var inversorIds = nodos
                .Where(nodo => nodo.InversorId.HasValue)
                .Select(nodo => nodo.InversorId!.Value)
                .Distinct()
                .ToList();

            var nombresInversores = inversorIds.Count == 0
                ? new Dictionary<int, string>()
                : await _mediadorNodoInversor.ObtenerNombresPorIdsAsync(inversorIds, cancellationToken);

            return nodos
                .Select(nodo => new NodoListadoDto
                {
                    Id = nodo.Id,
                    Nombre = nodo.Nombre,
                    UbicacionParque = nodo.UbicacionParque,
                    FechaCreacion = nodo.FechaCreacion,
                    EnergiaGenerada = nodo.EnergiaGenerada,
                    InversorId = nodo.InversorId,
                    NombreInversor = nodo.InversorId.HasValue &&
                                     nombresInversores.TryGetValue(nodo.InversorId.Value, out var nombreInversor)
                        ? nombreInversor
                        : null,
                    ParqueSolarId = nodo.ParqueSolarId
                })
                .ToList();
        }

        public async Task EliminarNodoAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException("Solo un administrador puede eliminar nodos.");
            }

            var nodo = await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);

            await _nodoRepository.EliminarAsync(nodo, cancellationToken);
        }

        public Task<IReadOnlyList<InversorDisponibleDto>> ObtenerInversoresDisponiblesAsync(
            CancellationToken cancellationToken = default)
        {
            return _mediadorNodoInversor.ObtenerDisponiblesAsync(cancellationToken);
        }

        private async Task ValidarDtoAsync(CrearNodoDto dto, CancellationToken cancellationToken)
        {
            var resultado = await _validadorCrear.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }

        private async Task ValidarDtoAsync(EditarNodoDto dto, CancellationToken cancellationToken)
        {
            var resultado = await _validadorEditar.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
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
    }
}
