using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        void Update(Order order);
    }
}