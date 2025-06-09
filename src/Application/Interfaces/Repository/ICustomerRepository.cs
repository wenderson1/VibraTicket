using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetByDocumentAsync(string document);
        void Update(Customer customer);
    }
}