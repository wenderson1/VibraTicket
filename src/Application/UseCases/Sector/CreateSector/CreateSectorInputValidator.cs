using FluentValidation;

namespace Application.UseCases.Sector.CreateSector
{
    public class CreateSectorInputValidator : AbstractValidator<CreateSectorInput>
    {
        public CreateSectorInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do setor � obrigat�rio.")
                .MaximumLength(100).WithMessage("O nome do setor deve ter no m�ximo 100 caracteres.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("A capacidade deve ser maior que zero.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("O pre�o deve ser maior ou igual a zero.");

            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("O ID do evento � obrigat�rio e deve ser maior que zero.");


            RuleFor(x => x.AvailableTickets)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade de ingressos dispon�veis deve ser maior ou igual a zero.")
                .LessThanOrEqualTo(x => x.Capacity).WithMessage("A quantidade de ingressos dispon�veis n�o pode ser maior que a capacidade.");
        }
    }
}