using FluentValidation;

namespace Application.UseCases.Customer.CreateCustomer
{
    public class CreateCustomerInputValidator : AbstractValidator<CreateCustomerInput>
    {
        public CreateCustomerInputValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("O nome completo é obrigatório.")
                .MaximumLength(200).WithMessage("O nome completo deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .MaximumLength(200).WithMessage("O e-mail deve ter no máximo 200 caracteres.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .MaximumLength(15).WithMessage("O CPF deve ter no máximo 15 caracteres.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .LessThan(DateTime.UtcNow).WithMessage("A data de nascimento não pode ser no futuro.");

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("O endereço deve ter no máximo 250 caracteres.");

            RuleFor(x => x.City)
                .MaximumLength(250).WithMessage("A cidade deve ter no máximo 250 caracteres.");

            RuleFor(x => x.State)
                .MaximumLength(250).WithMessage("O estado deve ter no máximo 250 caracteres.");

            RuleFor(x => x.ZipCode)
                .MaximumLength(250).WithMessage("O CEP deve ter no máximo 250 caracteres.");
        }
    }
}