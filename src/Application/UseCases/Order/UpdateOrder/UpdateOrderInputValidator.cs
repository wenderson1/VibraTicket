using FluentValidation;

namespace Application.UseCases.Order.UpdateOrder
{
    public class UpdateOrderInputValidator : AbstractValidator<UpdateOrderInput>
    {
        public UpdateOrderInputValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .When(x => x.Status.HasValue)
                .WithMessage("O status informado é inválido");
        }
    }
}