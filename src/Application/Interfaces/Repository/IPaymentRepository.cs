using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int id);
        void Update(Payment payment);
    }
}