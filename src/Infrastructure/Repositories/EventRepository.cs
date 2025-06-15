using Application.Interfaces.Repository;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Event @event)
        {
            await _context.Events.AddAsync(@event);
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Affiliate)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event?> GetByVenueAndDateRangeAsync(int venueId, DateTime startDate, DateTime endDate)
        {
            return await _context.Events
                .Where(e => e.VenueId == venueId && 
                           e.Status != EventStatus.Cancelled &&
                           ((e.StartDate <= startDate && e.EndDate >= startDate) || 
                            (e.StartDate <= endDate && e.EndDate >= endDate) ||
                            (e.StartDate >= startDate && e.EndDate <= endDate)))
                .FirstOrDefaultAsync();
        }

        public void Update(Event @event)
        {
            _context.Events.Update(@event);
        }
    }
}