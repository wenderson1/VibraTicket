using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(Guid id);
    }
}