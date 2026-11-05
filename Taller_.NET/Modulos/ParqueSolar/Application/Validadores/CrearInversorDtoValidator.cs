using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Validadores
{
    public class CrearInversorDtoValidator : AbstractValidator<CrearInversorDto>
    {
        public CrearInversorDtoValidator()
        {
            RuleFor(dto => dto.Nombre)
                .NotEmpty().WithMessage("El nombre del inversor es obligatorio.")
                .MaximumLength(InversorDomain.LongitudMaximaNombre)
                    .WithMessage($"El nombre del inversor no puede superar los {InversorDomain.LongitudMaximaNombre} caracteres.");

            RuleFor(dto => dto.NodoId)
                .GreaterThan(0).WithMessage("El nodo es obligatorio.");

            RuleFor(dto => dto.ParqueSolarId)
                .GreaterThan(0).WithMessage("El parque solar es obligatorio.");
        }
    }
}
