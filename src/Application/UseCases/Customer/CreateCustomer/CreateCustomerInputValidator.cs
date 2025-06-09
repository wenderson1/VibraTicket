using FluentValidation;

namespace Application.UseCases.Customer.CreateCustomer
{
    public class CreateCustomerInputValidator : AbstractValidator<CreateCustomerInput>
    {
        public CreateCustomerInputValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("O nome completo � obrigat�rio.")
                .MaximumLength(200).WithMessage("O nome completo deve ter no m�ximo 200 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome � obrigat�rio.")
                .MaximumLength(200).WithMessage("O nome deve ter no m�ximo 200 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail � obrigat�rio.")
                .MaximumLength(200).WithMessage("O e-mail deve ter no m�ximo 200 caracteres.")
                .EmailAddress().WithMessage("O e-mail informado n�o � v�lido.");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O CPF � obrigat�rio.")
                .MaximumLength(15).WithMessage("O CPF deve ter no m�ximo 15 caracteres.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("O telefone deve ter no m�ximo 20 caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("A data de nascimento � obrigat�ria.")
                .LessThan(DateTime.UtcNow).WithMessage("A data de nascimento n�o pode ser no futuro.");

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("O endere�o deve ter no m�ximo 250 caracteres.");

            RuleFor(x => x.City)
                .MaximumLength(250).WithMessage("A cidade deve ter no m�ximo 250 caracteres.");

            RuleFor(x => x.State)
                .MaximumLength(250).WithMessage("O estado deve ter no m�ximo 250 caracteres.");

            RuleFor(x => x.ZipCode)
                .MaximumLength(250).WithMessage("O CEP deve ter no m�ximo 250 caracteres.");
        }
    }
}