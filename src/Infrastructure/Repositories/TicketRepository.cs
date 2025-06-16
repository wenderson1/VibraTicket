using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByIdAsync(Guid id)
        {
            return await _context.Tickets
                .Include(t => t.Event)
                    .ThenInclude(e => e.Venue)
                .Include(t => t.Sector)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public void Update(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
        }
    }
}