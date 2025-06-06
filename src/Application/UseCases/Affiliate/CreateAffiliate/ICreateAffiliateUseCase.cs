using Application.Commons;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Affiliate.CreateAffiliate
{
    public interface ICreateAffiliateUseCase
    {
        Task<Result<int>> ExecuteAsync(CreateAffiliateInput input, CancellationToken cancellationToken);
    }
}
