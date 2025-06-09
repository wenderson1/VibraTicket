using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.IsActive);
        }

        public async Task<Customer?> GetByDocumentAsync(string document)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Document == document && c.IsActive);
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }
    }
}