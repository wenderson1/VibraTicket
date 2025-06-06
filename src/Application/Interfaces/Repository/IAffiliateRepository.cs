using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IAffiliateRepository
    {
        Task AddAsync(Affiliate affiliate);
        // Outros m�todos como GetByIdAsync, GetAllAsync, etc, podem ser adicionados conforme necess�rio
    }
}
