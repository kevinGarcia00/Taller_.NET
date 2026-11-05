using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Servicios
{
    public class InversorService : IInversorService
    {
        private const string MensajeNodoSinInversor = "El nodo no tiene un inversor asignado.";

        private readonly IInversorRepository _inversorRepository;
        private readonly INodoRepository _nodoRepository;
        private readonly IUsuarioActualProvider _usuarioActualProvider;
        private readonly IValidator<CrearInversorDto> _validadorCrear;
        private readonly IValidator<ModificarInversorDto> _validadorModificar;

        public InversorService(
            IInversorRepository inversorRepository,
            INodoRepository nodoRepository,
            IUsuarioActualProvider usuarioActualProvider,
            IValidator<CrearInversorDto> validadorCrear,
            IValidator<ModificarInversorDto> validadorModificar)
        {
            _inversorRepository = inversorRepository;
            _nodoRepository = nodoRepository;
            _usuarioActualProvider = usuarioActualProvider;
            _validadorCrear = validadorCrear;
            _validadorModificar = validadorModificar;
        }

        public async Task<int> CrearInversorAsync(CrearInversorDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(dto, cancellationToken);
            await AsegurarAdministradorAsync("Solo un administrador puede registrar inversores.", cancellationToken);

            var nodo = await ObtenerNodoDelParqueAsync(dto.NodoId, dto.ParqueSolarId, cancellationToken);

            if (nodo.TieneInversorAsignado())
            {
                throw new NodoYaTieneInversorException();
            }

            var inversor = new InversorDomain(dto.Nombre);
            var inversorId = await _inversorRepository.AgregarAsync(inversor, cancellationToken);

            nodo.AsignarInversor(inversor);
            await _nodoRepository.ActualizarAsync(nodo, cancellationToken);

            return inversorId;
        }

        public async Task<InversorDetalleDto?> ObtenerInversorDelNodoAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            await AsegurarAdministradorAsync("Solo un administrador puede ver el inversor.", cancellationToken);
            var nodo = await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);
            var inversor = await _inversorRepository.ObtenerPorNodoAsync(nodoId, cancellationToken);

            if (inversor is null)
            {
                return null;
            }

            return CrearDetalle(inversor, nodoId, nodo.ParqueSolarId);
        }

        public async Task<ModificarInversorDto> ObtenerParaEdicionAsync(
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            await AsegurarAdministradorAsync("Solo un administrador puede modificar inversores.", cancellationToken);
            var nodo = await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);
            var inversor = await ObtenerInversorAsignadoAsync(nodoId, cancellationToken);

            return new ModificarInversorDto
            {
                Id = inversor.Id,
                Nombre = inversor.Nombre,
                FechaCreacion = inversor.FechaCreacion,
                NodoId = nodoId,
                ParqueSolarId = nodo.ParqueSolarId
            };
        }

        public async Task ModificarInversorAsync(ModificarInversorDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(dto, cancellationToken);
            await AsegurarAdministradorAsync("Solo un administrador puede modificar inversores.", cancellationToken);
            await ObtenerNodoDelParqueAsync(dto.NodoId, dto.ParqueSolarId, cancellationToken);

            var inversor = await ObtenerInversorAsignadoAsync(dto.NodoId, cancellationToken);

            if (inversor.Id != dto.Id)
            {
                throw new InversorNoEncontradoException(dto.Id);
            }

            inversor.ActualizarNombre(dto.Nombre);
            await _inversorRepository.ActualizarAsync(inversor, cancellationToken);
        }

        public async Task EliminarInversorAsync(int nodoId, int parqueSolarId, CancellationToken cancellationToken = default)
        {
            await AsegurarAdministradorAsync("Solo un administrador puede eliminar inversores.", cancellationToken);
            var nodo = await ObtenerNodoDelParqueAsync(nodoId, parqueSolarId, cancellationToken);
            var inversor = await ObtenerInversorAsignadoAsync(nodoId, cancellationToken);

            nodo.QuitarInversor();
            await _nodoRepository.ActualizarAsync(nodo, cancellationToken);
            await _inversorRepository.EliminarAsync(inversor, cancellationToken);
        }

        private async Task<InversorDomain> ObtenerInversorAsignadoAsync(int nodoId, CancellationToken cancellationToken)
        {
            var inversor = await _inversorRepository.ObtenerPorNodoAsync(nodoId, cancellationToken);

            if (inversor is null)
            {
                throw new InversorNoEncontradoException(MensajeNodoSinInversor);
            }

            return inversor;
        }

        private static InversorDetalleDto CrearDetalle(InversorDomain inversor, int nodoId, int parqueSolarId)
        {
            return new InversorDetalleDto
            {
                Id = inversor.Id,
                Nombre = inversor.Nombre,
                FechaCreacion = inversor.FechaCreacion,
                NodoId = nodoId,
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

        private async Task ValidarDtoAsync(CrearInversorDto dto, CancellationToken cancellationToken)
        {
            var resultado = await _validadorCrear.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }

        private async Task ValidarDtoAsync(ModificarInversorDto dto, CancellationToken cancellationToken)
        {
            var resultado = await _validadorModificar.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }
    }
}
