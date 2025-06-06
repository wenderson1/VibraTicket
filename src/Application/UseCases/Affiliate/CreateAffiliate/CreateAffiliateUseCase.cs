using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Affiliate.CreateAffiliate
{
    public class CreateAffiliateUseCase : ICreateAffiliateUseCase
    {
        private readonly IUnitOfWork uow;
        private readonly IValidator<CreateAffiliateInput> validator;
        private readonly ILogger<CreateAffiliateUseCase> log;

        public CreateAffiliateUseCase(IUnitOfWork uow, IValidator<CreateAffiliateInput> validator, ILogger<CreateAffiliateUseCase> log)
        {
            this.uow = uow;
            this.validator = validator;
            this.log = log;
        }

        public async Task<Result<int>> ExecuteAsync(CreateAffiliateInput input, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation("Iniciando criação de Affiliate com nome: {0}", input.Name);
                ValidationResult validationResult = validator.Validate(input);
                if (!validationResult.IsValid)
                {
                    log.LogWarning("Validação do CreateAffiliateInput falhou: {@ValidationErrors}", validationResult.Errors);
                    Error appError = Error.FromValidationResult(validationResult);
                    return Result.Failure<int>(appError);
                }

                var affiliate = MapToAffiliate(input);
                await uow.Affiliates.AddAsync(affiliate);
                await uow.SaveChangesAsync(cancellationToken);

                log.LogInformation("Criado Affiliate: {0}", input);

                return affiliate.Id;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Erro inesperado durante a execução de CreateAffiliateUseCase para o Affiliate com nome {AffiliateName}.", input.Name);
                throw;
            }
        }

        private static Domain.Entities.Affiliate MapToAffiliate(CreateAffiliateInput input)
        {
            return new Domain.Entities.Affiliate
            {
                Name = input.Name,
                FullName = input.FullName,
                Document = input.Document,
                Email = input.Email,
                Phone = input.Phone,
                BankName = input.BankName,
                BankAccount = input.BankAccount,
                BankBranch = input.BankBranch,
                DefaultCommissionRate = input.DefaultCommissionRate
            };
        }
    }
}
