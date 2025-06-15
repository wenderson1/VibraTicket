using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Affiliate?> GetByIdAsync(int id)
        {
            return await _context.Set<Affiliate>().FindAsync(id);
        }

        public async Task<Affiliate?> GetByDocumentAsync(string document)
        {
            return await _context.Set<Affiliate>()
                .FirstOrDefaultAsync(a => a.Document == document);
        }

        public void Update(Affiliate affiliate)
        {
            _context.Set<Affiliate>().Update(affiliate);
        }
    }
}
