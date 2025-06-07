using Application.Commons;
using System.Threading.Tasks;

namespace Application.UseCases.Affiliate.UpdateAffiliate
{
    public interface IUpdateAffiliateUseCase
    {
        Task<Result<bool>> Execute(int id, UpdateAffiliateInput input);
    }
}