using FluentValidation;

namespace Application.UseCases.Customer.UpdateCustomer
{
    public class UpdateCustomerInputValidator : AbstractValidator<UpdateCustomerInput>
    {
        public UpdateCustomerInputValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(200).WithMessage("O nome completo deve ter no máximo 200 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.FullName));

            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("O e-mail informado não é válido.")
                .MaximumLength(200).WithMessage("O e-mail deve ter no máximo 200 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Document)
                .MaximumLength(15).WithMessage("O documento deve ter no máximo 15 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Document));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.UtcNow).WithMessage("A data de nascimento não pode ser no futuro.")
                .When(x => x.BirthDate.HasValue);

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("O endereço deve ter no máximo 250 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.City)
                .MaximumLength(250).WithMessage("A cidade deve ter no máximo 250 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.City));

            RuleFor(x => x.State)
                .MaximumLength(250).WithMessage("O estado deve ter no máximo 250 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.State));

            RuleFor(x => x.ZipCode)
                .MaximumLength(250).WithMessage("O CEP deve ter no máximo 250 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.ZipCode));
        }
    }
}