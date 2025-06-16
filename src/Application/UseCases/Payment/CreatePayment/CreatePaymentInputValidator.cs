using FluentValidation;

namespace Application.UseCases.Payment.CreatePayment
{
    public class CreatePaymentInputValidator : AbstractValidator<CreatePaymentInput>
    {
        public CreatePaymentInputValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("O ID do pedido é obrigatório");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("O valor do pagamento deve ser maior que zero");

            RuleFor(x => x.Method)
                .IsInEnum()
                .WithMessage("O método de pagamento informado é inválido");

            RuleFor(x => x.TransactionId)
                .MaximumLength(100)
                .When(x => x.TransactionId != null)
                .WithMessage("O ID da transação deve ter no máximo 100 caracteres");

            RuleFor(x => x.GatewayResponse)
                .MaximumLength(5000)
                .When(x => x.GatewayResponse != null)
                .WithMessage("A resposta do gateway deve ter no máximo 5000 caracteres");
        }
    }
}