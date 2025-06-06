using FluentValidation;

namespace Application.UseCases.Affiliate.CreateAffiliate
{
    public class CreateAffiliateInputValidator : AbstractValidator<CreateAffiliateInput>
    {
        public CreateAffiliateInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do afiliado é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do afiliado deve ter no máximo 100 caracteres.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("A razão social ou nome completo é obrigatória.")
                .MaximumLength(200).WithMessage("A razão social ou nome completo deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O documento (CNPJ ou CPF) é obrigatório.")
                .MaximumLength(20).WithMessage("O documento deve ter no máximo 20 caracteres.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("O e-mail informado não é válido.")
                .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");

            RuleFor(x => x.BankName)
                .MaximumLength(100).WithMessage("O nome do banco deve ter no máximo 100 caracteres.");

            RuleFor(x => x.BankAccount)
                .MaximumLength(30).WithMessage("A conta bancária deve ter no máximo 30 caracteres.");

            RuleFor(x => x.BankBranch)
                .MaximumLength(20).WithMessage("A agência bancária deve ter no máximo 20 caracteres.");

            RuleFor(x => x.DefaultCommissionRate)
                .GreaterThanOrEqualTo(0).WithMessage("A comissão padrão deve ser maior ou igual a zero.");
        }
    }
}
