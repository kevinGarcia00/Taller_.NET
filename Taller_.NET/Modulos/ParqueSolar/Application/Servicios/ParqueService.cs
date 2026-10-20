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
        private readonly IUsuarioActualProvider _usuarioActualProvider;
        private readonly IValidator<CrearParqueDto> _validador;

        public ParqueService(
            IParqueRepository parqueRepository,
            IUsuarioActualProvider usuarioActualProvider,
            IValidator<CrearParqueDto> validador)
        {
            _parqueRepository = parqueRepository;
            _usuarioActualProvider = usuarioActualProvider;
            _validador = validador;
        }

        public async Task<int> CrearParqueAsync(CrearParqueDto dto, CancellationToken cancellationToken = default)
        {
            await ValidarDtoAsync(dto, cancellationToken);

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

        private async Task ValidarDtoAsync(CrearParqueDto dto, CancellationToken cancellationToken)
        {
            var resultado = await _validador.ValidateAsync(dto, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }
    }
}
