using System;
using FluentValidation;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Domain;

namespace Taller_.NET.Modulos.Historico.Application.Validadores
{
    public class CrearHistoricoMantenibilidadDtoValidator : AbstractValidator<CrearHistoricoMantenibilidadDto>
    {
        public CrearHistoricoMantenibilidadDtoValidator()
        {
            RuleFor(dto => dto.IdMantenedor)
                .NotEmpty().WithMessage("El usuario de mantenimiento es obligatorio.");

            RuleFor(dto => dto.Fecha)
                .NotEqual(default(DateTime)).WithMessage("La fecha es obligatoria.")
                .Must(fecha => fecha.Date <= DateTime.Today)
                    .WithMessage("La fecha del registro no puede ser posterior al dia de la carga.");

            RuleFor(dto => dto.EstadoResultante)
                .IsInEnum()
                    .WithMessage("El estado resultante debe ser Sucio, Roto, Encendido, Deteriorado o Apagado.");

            RuleFor(dto => dto.Observaciones)
                .NotEmpty().WithMessage("Las observaciones son obligatorias.")
                .MaximumLength(HistoricoMantenibilidad.LongitudMaximaObservaciones)
                    .WithMessage($"Las observaciones no pueden superar los {HistoricoMantenibilidad.LongitudMaximaObservaciones} caracteres.");

            RuleFor(dto => dto.PanelId)
                .GreaterThan(0).WithMessage("El panel es obligatorio.");

            RuleFor(dto => dto.NodoId)
                .GreaterThan(0).WithMessage("El nodo es obligatorio.");

            RuleFor(dto => dto.ParqueSolarId)
                .GreaterThan(0).WithMessage("El parque solar es obligatorio.");
        }
    }
}
