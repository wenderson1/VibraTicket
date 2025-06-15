using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Affiliate.GetAffiliateByDocument
{
    public class GetAffiliateByDocumentQuery : IGetAffiliateByDocumentQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetAffiliateByDocumentQuery> _log;

        public GetAffiliateByDocumentQuery(IUnitOfWork uow, ILogger<GetAffiliateByDocumentQuery> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<GetAffiliateByDocumentOutput>> ExecuteAsync(string document, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Buscando affiliate com Document: {0}", document);
                var affiliate = await _uow.Affiliates.GetByDocumentAsync(document);
                
                if (affiliate == null)
                {
                    _log.LogWarning("Affiliate não encontrado com Document: {0}", document);
                    return Result.Failure<GetAffiliateByDocumentOutput>(Error.NotFound("Não foi encontrado o Afiliado"));
                }

                var output = new GetAffiliateByDocumentOutput
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

                _log.LogInformation("Affiliate encontrado com sucesso. Document: {0}", document);
                return Result.Success(output);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao buscar affiliate com Document: {AffiliateDocument}", document);
                return Result.Failure<GetAffiliateByDocumentOutput>(Error.InternalError);
            }
        }
    }
}