using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IAffiliateRepository
    {
        Task AddAsync(Affiliate affiliate);
        Task<Affiliate?> GetByIdAsync(int id);
        void Update(Affiliate affiliate);
    }
}
