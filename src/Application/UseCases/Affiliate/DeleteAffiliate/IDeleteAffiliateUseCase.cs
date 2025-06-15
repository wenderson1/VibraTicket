using Application.Commons;

namespace Application.UseCases.Affiliate.DeleteAffiliate
{
    public interface IDeleteAffiliateUseCase
    {
        Task<Result<bool>> Execute(int id);
    }
}