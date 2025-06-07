using FluentValidation;

namespace Application.UseCases.Affiliate.UpdateAffiliate
{
    public class UpdateAffiliateInputValidator : AbstractValidator<UpdateAffiliateInput>
    {
        public UpdateAffiliateInputValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("O nome do afiliado deve ter no m�ximo 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.FullName)
                .MaximumLength(200).WithMessage("A raz�o social ou nome completo deve ter no m�ximo 200 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.FullName));

            RuleFor(x => x.Document)
                .MaximumLength(20).WithMessage("O documento deve ter no m�ximo 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Document));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("O e-mail informado n�o � v�lido.")
                .MaximumLength(150).WithMessage("O e-mail deve ter no m�ximo 150 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("O telefone deve ter no m�ximo 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.BankName)
                .MaximumLength(100).WithMessage("O nome do banco deve ter no m�ximo 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.BankName));

            RuleFor(x => x.BankAccount)
                .MaximumLength(30).WithMessage("A conta banc�ria deve ter no m�ximo 30 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.BankAccount));

            RuleFor(x => x.BankBranch)
                .MaximumLength(20).WithMessage("A ag�ncia banc�ria deve ter no m�ximo 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.BankBranch));

            RuleFor(x => x.DefaultCommissionRate)
                .GreaterThanOrEqualTo(0).WithMessage("A comiss�o padr�o deve ser maior ou igual a zero.")
                .When(x => x.DefaultCommissionRate.HasValue);
        }
    }
}