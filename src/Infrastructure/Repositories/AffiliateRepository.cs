using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AffiliateRepository : IAffiliateRepository
    {
        private readonly DbContext _context;
        public AffiliateRepository(DbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Affiliate affiliate)
        {
            await _context.Set<Affiliate>().AddAsync(affiliate);
        }
    }
}
