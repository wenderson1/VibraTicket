using FluentValidation;

namespace Application.UseCases.Sector.UpdateSector
{
    public class UpdateSectorInputValidator : AbstractValidator<UpdateSectorInput>
    {
        public UpdateSectorInputValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .WithMessage("O nome do setor deve ter no máximo 100 caracteres.")
                .When(x => x.Name is not null);

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A capacidade deve ser maior ou igual a zero.")
                .When(x => x.Capacity is not null);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O preço deve ser maior ou igual a zero.")
                .When(x => x.Price is not null);

            RuleFor(x => x.EventId)
                .GreaterThan(0)
                .WithMessage("O ID do evento deve ser maior que zero.")
                .When(x => x.EventId is not null);

            RuleFor(x => x.AvailableTickets)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A quantidade de ingressos disponíveis deve ser maior ou igual a zero.")
                .When(x => x.AvailableTickets is not null);
        }
    }
}
