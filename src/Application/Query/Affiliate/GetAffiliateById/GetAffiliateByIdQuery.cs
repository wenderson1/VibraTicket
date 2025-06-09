using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Affiliate.GetAffiliateById
{
    public class GetAffiliateByIdQuery : IGetAffiliateByIdQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetAffiliateByIdQuery> _log;

        public GetAffiliateByIdQuery(IUnitOfWork uow, ILogger<GetAffiliateByIdQuery> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<GetAffiliateByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Buscando affiliate com ID: {0}", id);
                var affiliate = await _uow.Affiliates.GetByIdAsync(id);
                
                if (affiliate == null)
                {
                    _log.LogWarning("Affiliate não encontrado com ID: {0}", id);
                    return Result.Failure<GetAffiliateByIdOutput>(Error.NotFound("Não foi encontrado o Afiliado"));
                }

                var output = new GetAffiliateByIdOutput
                {
                    Id = affiliate.Id,
                    Name = affiliate.Name,
                    FullName = affiliate.FullName,
                    Document = affiliate.Document,
                    Email = affiliate.Email,
                    Phone = affiliate.Phone,
                    BankName = affiliate.BankName,
                    BankAccount = affiliate.BankAccount,
                    BankBranch = affiliate.BankBranch,
                    DefaultCommissionRate = affiliate.DefaultCommissionRate,
                    CreatedAt = affiliate.CreatedAt
                };

                _log.LogInformation("Affiliate encontrado com sucesso. ID: {0}", id);
                return Result.Success(output);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao buscar affiliate com ID: {AffiliateId}", id);
                return Result.Failure<GetAffiliateByIdOutput>(Error.InternalError);
            }
        }
    }
}