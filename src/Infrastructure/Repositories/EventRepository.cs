using Application.Interfaces.Repository;
using Domain.Entities;
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
            return await _context.Events.FindAsync(id);
        }

        public void Update(Event @event)
        {
            _context.Events.Update(@event);
        }
    }
}