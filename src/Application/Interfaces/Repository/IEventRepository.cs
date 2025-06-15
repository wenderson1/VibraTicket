using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IEventRepository
    {
        Task AddAsync(Event @event);
        Task<Event?> GetByIdAsync(int id);
        void Update(Event @event);
        Task<Event?> GetByVenueAndDateRangeAsync(int venueId, DateTime startDate, DateTime endDate);
    }
}