using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IAffiliateRepository
    {
        Task AddAsync(Affiliate affiliate);
        // Outros métodos como GetByIdAsync, GetAllAsync, etc, podem ser adicionados conforme necessário
    }
}
