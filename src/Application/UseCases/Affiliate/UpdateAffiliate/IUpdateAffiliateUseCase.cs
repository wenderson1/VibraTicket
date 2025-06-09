using Application.Commons;

namespace Application.UseCases.Affiliate.UpdateAffiliate
{
    public interface IUpdateAffiliateUseCase
    {
        Task<Result<bool>> Execute(int id, UpdateAffiliateInput input);
    }
}