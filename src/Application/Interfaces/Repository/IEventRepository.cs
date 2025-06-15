using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IEventRepository
    {
        Task AddAsync(Event @event);
        Task<Event?> GetByIdAsync(int id);
        Task<Event?> GetByVenueAndDateRangeAsync(int venueId, DateTime startDate, DateTime endDate);
        Task<bool> HasTicketsAsync(int eventId);
        void Delete(Event @event);
        void Update(Event @event);
    }
}