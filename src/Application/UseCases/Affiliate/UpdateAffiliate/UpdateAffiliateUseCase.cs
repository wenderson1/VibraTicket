using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Affiliate.UpdateAffiliate
{
    public class UpdateAffiliateUseCase : IUpdateAffiliateUseCase
    {
        private readonly IUnitOfWork uow;
        private readonly IValidator<UpdateAffiliateInput> validator;
        private readonly ILogger<UpdateAffiliateUseCase> log;

        public UpdateAffiliateUseCase(
            IUnitOfWork uow,
            IValidator<UpdateAffiliateInput> validator,
            ILogger<UpdateAffiliateUseCase> log)
        {
            this.uow = uow;
            this.validator = validator;
            this.log = log;
        }

        public async Task<Result<bool>> Execute(int id, UpdateAffiliateInput input)
        {
            try
            {
                log.LogInformation("Iniciando atualização de Affiliate ID: {0}", id);
                
                ValidationResult validationResult = validator.Validate(input);
                if (!validationResult.IsValid)
                {
                    log.LogWarning("Validação do UpdateAffiliateInput falhou: {@ValidationErrors}", validationResult.Errors);
                    return Result.Failure<bool>(Error.FromValidationResult(validationResult));
                }

                var affiliate = await uow.Affiliates.GetByIdAsync(id);
                if (affiliate is null)
                {
                    log.LogWarning("Affiliate não encontrado com ID: {0}", id);
                    return Result.Failure<bool>(Error.NotFound("Não foi encontrado o Afiliado"));
                }

                UpdateAffiliateProperties(affiliate, input);
                uow.Affiliates.Update(affiliate);
                await uow.SaveChangesAsync();

                log.LogInformation("Affiliate atualizado com sucesso. ID: {0}", id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Erro inesperado durante a atualização do Affiliate ID: {AffiliateId}", id);
                return Result.Failure<bool>(Error.InternalError);
            }
        }

        private static void UpdateAffiliateProperties(Domain.Entities.Affiliate affiliate, UpdateAffiliateInput input)
        {
            if (!string.IsNullOrEmpty(input.Name))
                affiliate.Name = input.Name;

            if (!string.IsNullOrEmpty(input.FullName))
                affiliate.FullName = input.FullName;

            if (!string.IsNullOrEmpty(input.Document))
                affiliate.Document = input.Document;

            affiliate.Email = input.Email;
            affiliate.Phone = input.Phone;
            affiliate.BankName = input.BankName;
            affiliate.BankAccount = input.BankAccount;
            affiliate.BankBranch = input.BankBranch;

            if (input.DefaultCommissionRate.HasValue)
                affiliate.DefaultCommissionRate = input.DefaultCommissionRate.Value;
        }
    }
}