using Application.Commons;

namespace Application.Query.Affiliate.GetAffiliateById
{
    public interface IGetAffiliateByIdQuery
    {
        Task<Result<GetAffiliateByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default);
    }
}