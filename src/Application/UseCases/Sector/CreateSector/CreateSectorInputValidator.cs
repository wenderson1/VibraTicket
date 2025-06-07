using FluentValidation;

namespace Application.UseCases.Sector.CreateSector
{
    public class CreateSectorInputValidator : AbstractValidator<CreateSectorInput>
    {
        public CreateSectorInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do setor é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do setor deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("A capacidade deve ser maior que zero.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("O preço deve ser maior ou igual a zero.");

            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("O ID do evento é obrigatório e deve ser maior que zero.");


            RuleFor(x => x.AvailableTickets)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade de ingressos disponíveis deve ser maior ou igual a zero.")
                .LessThanOrEqualTo(x => x.Capacity).WithMessage("A quantidade de ingressos disponíveis não pode ser maior que a capacidade.");
        }
    }
}