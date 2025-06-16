using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);
        void Update(Payment payment);
    }
}