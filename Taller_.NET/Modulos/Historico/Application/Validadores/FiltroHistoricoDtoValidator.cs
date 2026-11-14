using FluentValidation;
using Taller_.NET.Modulos.Historico.Application.Dtos;

namespace Taller_.NET.Modulos.Historico.Application.Validadores
{
    public class FiltroHistoricoDtoValidator : AbstractValidator<FiltroHistoricoDto>
    {
        public FiltroHistoricoDtoValidator()
        {
            RuleFor(filtro => filtro.Referencia)
                .NotEmpty()
                    .WithMessage("La fecha a buscar es obligatoria.");
        }
    }
}
