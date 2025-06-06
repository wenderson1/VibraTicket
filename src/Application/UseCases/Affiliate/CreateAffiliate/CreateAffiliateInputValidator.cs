using FluentValidation;

namespace Application.UseCases.Affiliate.CreateAffiliate
{
    public class CreateAffiliateInputValidator : AbstractValidator<CreateAffiliateInput>
    {
        public CreateAffiliateInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do afiliado � obrigat�rio.")
                .MaximumLength(100).WithMessage("O nome do afiliado deve ter no m�ximo 100 caracteres.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("A raz�o social ou nome completo � obrigat�ria.")
                .MaximumLength(200).WithMessage("A raz�o social ou nome completo deve ter no m�ximo 200 caracteres.");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O documento (CNPJ ou CPF) � obrigat�rio.")
                .MaximumLength(20).WithMessage("O documento deve ter no m�ximo 20 caracteres.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("O e-mail informado n�o � v�lido.")
                .MaximumLength(150).WithMessage("O e-mail deve ter no m�ximo 150 caracteres.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("O telefone deve ter no m�ximo 20 caracteres.");

            RuleFor(x => x.BankName)
                .MaximumLength(100).WithMessage("O nome do banco deve ter no m�ximo 100 caracteres.");

            RuleFor(x => x.BankAccount)
                .MaximumLength(30).WithMessage("A conta banc�ria deve ter no m�ximo 30 caracteres.");

            RuleFor(x => x.BankBranch)
                .MaximumLength(20).WithMessage("A ag�ncia banc�ria deve ter no m�ximo 20 caracteres.");

            RuleFor(x => x.DefaultCommissionRate)
                .GreaterThanOrEqualTo(0).WithMessage("A comiss�o padr�o deve ser maior ou igual a zero.");
        }
    }
}
