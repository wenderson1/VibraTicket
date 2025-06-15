using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IAffiliateRepository
    {
        Task AddAsync(Affiliate affiliate);
        Task<Affiliate?> GetByIdAsync(int id);
        Task<Affiliate?> GetByDocumentAsync(string document);
        void Update(Affiliate affiliate);
    }
}
