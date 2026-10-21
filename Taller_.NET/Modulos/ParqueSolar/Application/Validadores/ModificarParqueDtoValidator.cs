using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Validadores
{
    public class ModificarParqueDtoValidator : AbstractValidator<ModificarParqueDto>
    {
        public ModificarParqueDtoValidator()
        {
            RuleFor(dto => dto.Nombre)
                .NotEmpty().WithMessage("El nombre del parque es obligatorio.")
                .MaximumLength(Parque.LongitudMaximaNombre)
                    .WithMessage($"El nombre del parque no puede superar los {Parque.LongitudMaximaNombre} caracteres.");

            RuleFor(dto => dto.Ubicacion)
                .NotEmpty().WithMessage("La ubicacion del parque es obligatoria.")
                .MaximumLength(Parque.LongitudMaximaUbicacion)
                    .WithMessage($"La ubicacion del parque no puede superar los {Parque.LongitudMaximaUbicacion} caracteres.");

            RuleFor(dto => dto.CantMaxNodos)
                .GreaterThan(0).WithMessage("La cantidad maxima de nodos debe ser mayor a cero.");
        }
    }
}
