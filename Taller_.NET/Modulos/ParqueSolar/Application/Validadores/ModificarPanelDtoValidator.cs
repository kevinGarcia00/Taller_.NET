using FluentValidation;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Validadores
{
    public class ModificarPanelDtoValidator : AbstractValidator<ModificarPanelDto>
    {
        public ModificarPanelDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .GreaterThan(0).WithMessage("El panel es obligatorio.");

            RuleFor(dto => dto.Nombre)
                .NotEmpty().WithMessage("El nombre del panel es obligatorio.")
                .MaximumLength(PanelDomain.LongitudMaximaNombre)
                    .WithMessage($"El nombre del panel no puede superar los {PanelDomain.LongitudMaximaNombre} caracteres.");

            RuleFor(dto => dto.Estado)
                .IsInEnum()
                    .WithMessage("El estado del panel debe ser Sucio, Roto, Encendido, Deteriorado o Apagado.");

            RuleFor(dto => dto.Potencia)
                .GreaterThan(0).WithMessage("La potencia del panel debe ser mayor a cero.");

            RuleFor(dto => dto.VidaUtil)
                .NotEmpty().WithMessage("La vida util del panel es obligatoria.")
                .MaximumLength(PanelDomain.LongitudMaximaVidaUtil)
                    .WithMessage($"La vida util del panel no puede superar los {PanelDomain.LongitudMaximaVidaUtil} caracteres.");

            RuleFor(dto => dto.NodoId)
                .GreaterThan(0).WithMessage("El nodo es obligatorio.");

            RuleFor(dto => dto.ParqueSolarId)
                .GreaterThan(0).WithMessage("El parque solar es obligatorio.");
        }
    }
}
