using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Servicios
{
    public class ParqueService : IParqueService
    {
        private readonly IParqueRepository _parqueRepository;
        private readonly IMediadorParqueNodo _mediadorParqueNodo;
        private readonly IUsuarioActualProvider _usuarioActualProvider;
        private readonly IValidator<CrearParqueDto> _validadorCrear;
        private readonly IValidator<ModificarParqueDto> _validadorModificar;

        public ParqueService(
            IParqueRepository parqueRepository,
            IMediadorParqueNodo mediadorParqueNodo,
            IUsuarioActualProvider usuarioActualProvider,
            IValidator<CrearParqueDto> validadorCrear,
            IValidator<ModificarParqueDto> validadorModificar)
        {
            _parqueRepository = parqueRepository;
            _mediadorParqueNodo = mediadorParqueNodo;
            _usuarioActualProvider = usuarioActualProvider;
            _validadorCrear = validadorCrear;
            _validadorModificar = validadorModificar;
        }

        public async Task<int> CrearParqueAsync(CrearParqueDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(_validadorCrear, dto, cancellationToken);

            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException("Solo un administrador puede registrar parques solares.");
            }

            if (await _parqueRepository.ExisteParqueConNombreAsync(dto.Nombre, cancellationToken))
            {
                throw new ParqueNombreDuplicadoException(dto.Nombre);
            }

            var parque = new Parque(dto.Nombre, dto.Ubicacion, dto.CantMaxNodos);

            return await _parqueRepository.AgregarAsync(parque, cancellationToken);
        }

        public async Task<int> ModificarParqueAsync(
            int parqueSolarId,
            ModificarParqueDto dto,
            CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(_validadorModificar, dto, cancellationToken);

            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException("Solo un administrador puede modificar parques solares.");
            }

            var parque = _parqueRepository.ObtenerPorId(parqueSolarId)
                ?? throw new ParqueNoEncontradoException(parqueSolarId);

            var nombreNormalizado = dto.Nombre.Trim();

            if (await _parqueRepository.ExisteOtroParqueConNombreAsync(nombreNormalizado, parque.Id, cancellationToken))
            {
                throw new ParqueNombreDuplicadoException(nombreNormalizado);
            }

            parque.ValidarCapacidadNodos(_mediadorParqueNodo.ObtenerNodosPorParque(parque.Id).Count, dto.CantMaxNodos);

            parque.Modificar(dto.Nombre, dto.Ubicacion, dto.CantMaxNodos);

            await _parqueRepository.ActualizarAsync(parque, cancellationToken);

            return parque.Id;
        }

        public async Task EliminarParqueAsync(int parqueSolarId, CancellationToken cancellationToken = default)
        {
            if (!await _usuarioActualProvider.EsAdministradorAsync(cancellationToken))
            {
                throw new UsuarioNoAutorizadoException("Solo un administrador puede eliminar parques solares.");
            }

            var parque = _parqueRepository.ObtenerPorId(parqueSolarId)
                ?? throw new ParqueNoEncontradoException(parqueSolarId);

            if (_mediadorParqueNodo.ObtenerNodosPorParque(parque.Id).Count > 0)
            {
                throw new ParqueConNodosAsociadosException(parqueSolarId);
            }

            await _parqueRepository.EliminarAsync(parque, cancellationToken);
        }

        public IReadOnlyList<ParqueListadoDto> ObtenerParquesSync()
        {
            return _parqueRepository.ObtenerTodos()
                .Select(parque => new ParqueListadoDto
                {
                    Id = parque.Id,
                    Nombre = parque.Nombre,
                    Ubicacion = parque.Ubicacion,
                    CantidadNodosActuales = parque.ObtenerCantidadNodosActuales(
                        _mediadorParqueNodo.ObtenerNodosPorParque(parque.Id).Count)
                })
                .ToList();
        }

        public ParqueDetalleDto ObtenerParqueSync(int parqueSolarId)
        {
            var parque = _parqueRepository.ObtenerPorId(parqueSolarId)
                ?? throw new ParqueNoEncontradoException(parqueSolarId);

            var nodos = _mediadorParqueNodo.ObtenerNodosPorParque(parque.Id);

            return new ParqueDetalleDto
            {
                Id = parque.Id,
                Nombre = parque.Nombre,
                Ubicacion = parque.Ubicacion,
                CantMaxNodos = parque.CantMaxNodos,
                CantidadNodosActuales = parque.ObtenerCantidadNodosActuales(nodos.Count),
                FechaCreacion = parque.FechaCreacion,
                ListaNodos = nodos
            };
        }

        private static async Task ValidarDtoAsync<TDto>(
            IValidator<TDto> validador,
            TDto dto,
            CancellationToken cancellationToken)
        {
            var resultado = await validador.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }
    }
}
