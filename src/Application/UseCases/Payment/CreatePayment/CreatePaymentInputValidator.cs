using FluentValidation;

namespace Application.UseCases.Payment.CreatePayment
{
    public class CreatePaymentInputValidator : AbstractValidator<CreatePaymentInput>
    {
        public CreatePaymentInputValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("O ID do pedido � obrigat�rio");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("O valor do pagamento deve ser maior que zero");

            RuleFor(x => x.Method)
                .IsInEnum()
                .WithMessage("O m�todo de pagamento informado � inv�lido");

            RuleFor(x => x.TransactionId)
                .MaximumLength(100)
                .When(x => x.TransactionId != null)
                .WithMessage("O ID da transa��o deve ter no m�ximo 100 caracteres");

            RuleFor(x => x.GatewayResponse)
                .MaximumLength(5000)
                .When(x => x.GatewayResponse != null)
                .WithMessage("A resposta do gateway deve ter no m�ximo 5000 caracteres");
        }
    }
}