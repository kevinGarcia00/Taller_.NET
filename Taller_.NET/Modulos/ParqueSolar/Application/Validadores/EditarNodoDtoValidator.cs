using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Validadores
{
    public class EditarNodoDtoValidator : AbstractValidator<EditarNodoDto>
    {
        public EditarNodoDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .GreaterThan(0).WithMessage("El identificador del nodo es obligatorio.");

            RuleFor(dto => dto.Nombre)
                .NotEmpty().WithMessage("El nombre del nodo es obligatorio.")
                .MaximumLength(NodoDomain.LongitudMaximaNombre)
                    .WithMessage($"El nombre del nodo no puede superar los {NodoDomain.LongitudMaximaNombre} caracteres.");

            RuleFor(dto => dto.UbicacionParque)
                .NotEmpty().WithMessage("La ubicacion del nodo en el parque es obligatoria.")
                .MaximumLength(NodoDomain.LongitudMaximaUbicacionParque)
                    .WithMessage($"La ubicacion del nodo no puede superar los {NodoDomain.LongitudMaximaUbicacionParque} caracteres.");

            RuleFor(dto => dto.InversorId)
                .GreaterThan(0).WithMessage("El inversor seleccionado no es valido.")
                .When(dto => dto.InversorId.HasValue);

            RuleFor(dto => dto.ParqueSolarId)
                .GreaterThan(0).WithMessage("El parque solar es obligatorio.");
        }
    }
}
