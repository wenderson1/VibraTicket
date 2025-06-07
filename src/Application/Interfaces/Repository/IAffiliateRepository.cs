using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IAffiliateRepository
    {
        Task AddAsync(Affiliate affiliate);
        Task<Affiliate?> GetByIdAsync(int id);
        void Update(Affiliate affiliate);
    }
}
