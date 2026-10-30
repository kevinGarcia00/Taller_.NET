using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Validadores
{
    public class CrearNodoDtoValidator : AbstractValidator<CrearNodoDto>
    {
        public CrearNodoDtoValidator()
        {
            RuleFor(dto => dto.Nombre)
                .NotEmpty().WithMessage("El nombre del nodo es obligatorio.")
                .MaximumLength(NodoDomain.LongitudMaximaNombre)
                    .WithMessage($"El nombre del nodo no puede superar los {NodoDomain.LongitudMaximaNombre} caracteres.");

            RuleFor(dto => dto.UbicacionParque)
                .NotEmpty().WithMessage("La ubicacion del nodo en el parque es obligatoria.")
                .MaximumLength(NodoDomain.LongitudMaximaUbicacionParque)
                    .WithMessage($"La ubicacion del nodo no puede superar los {NodoDomain.LongitudMaximaUbicacionParque} caracteres.");

            RuleFor(dto => dto.ParqueSolarId)
                .GreaterThan(0).WithMessage("El parque solar es obligatorio.");

            RuleFor(dto => dto.InversorId)
                .GreaterThan(0).When(dto => dto.InversorId.HasValue)
                    .WithMessage("El inversor seleccionado no es valido.");
        }
    }
}
