using FluentValidation;

namespace Application.UseCases.Order.CreateOrder
{
    public class CreateOrderInputValidator : AbstractValidator<CreateOrderInput>
    {
        public CreateOrderInputValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("O ID do cliente � obrigat�rio");

            RuleFor(x => x.TicketIds)
                .NotEmpty()
                .WithMessage("� necess�rio informar pelo menos um ingresso")
                .ForEach(x => x
                    .NotEmpty()
                    .WithMessage("O ID do ingresso � obrigat�rio"));
        }
    }
}