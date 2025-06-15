using Application.Commons;

namespace Application.Query.Affiliate.GetAffiliateByDocument
{
    public interface IGetAffiliateByDocumentQuery
    {
        Task<Result<GetAffiliateByDocumentOutput>> ExecuteAsync(string document, CancellationToken cancellationToken = default);
    }
}