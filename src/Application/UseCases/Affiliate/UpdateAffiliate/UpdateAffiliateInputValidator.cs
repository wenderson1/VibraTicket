using FluentValidation;

namespace Application.UseCases.Affiliate.UpdateAffiliate
{
    public class UpdateAffiliateInputValidator : AbstractValidator<UpdateAffiliateInput>
    {
        public UpdateAffiliateInputValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("O nome do afiliado deve ter no máximo 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.FullName)
                .MaximumLength(200).WithMessage("A razão social ou nome completo deve ter no máximo 200 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.FullName));

            RuleFor(x => x.Document)
                .MaximumLength(20).WithMessage("O documento deve ter no máximo 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Document));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("O e-mail informado não é válido.")
                .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.BankName)
                .MaximumLength(100).WithMessage("O nome do banco deve ter no máximo 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.BankName));

            RuleFor(x => x.BankAccount)
                .MaximumLength(30).WithMessage("A conta bancária deve ter no máximo 30 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.BankAccount));

            RuleFor(x => x.BankBranch)
                .MaximumLength(20).WithMessage("A agência bancária deve ter no máximo 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.BankBranch));

            RuleFor(x => x.DefaultCommissionRate)
                .GreaterThanOrEqualTo(0).WithMessage("A comissão padrão deve ser maior ou igual a zero.")
                .When(x => x.DefaultCommissionRate.HasValue);
        }
    }
}