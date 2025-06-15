using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Affiliate.DeleteAffiliate
{
    public class DeleteAffiliateUseCase : IDeleteAffiliateUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<DeleteAffiliateUseCase> _log;

        public DeleteAffiliateUseCase(IUnitOfWork uow, ILogger<DeleteAffiliateUseCase> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<bool>> Execute(int id)
        {
            try
            {
                _log.LogInformation("Tentando deletar (soft delete) affiliate com ID: {0}", id);
                var affiliate = await _uow.Affiliates.GetByIdAsync(id);
                
                if (affiliate is null)
                {
                    _log.LogWarning("Affiliate não encontrado com ID: {0}", id);
                    return Result.Failure<bool>(Error.NotFound("Não foi encontrado o Afiliado"));
                }

                affiliate.IsActive = false;
                _uow.Affiliates.Update(affiliate);
                await _uow.SaveChangesAsync();

                _log.LogInformation("Affiliate marcado como inativo com sucesso. ID: {0}", id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao deletar affiliate com ID: {AffiliateId}", id);
                return Result.Failure<bool>(Error.InternalError);
            }
        }
    }
}