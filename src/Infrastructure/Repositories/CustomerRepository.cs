using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private IQueryable<Customer> ActiveCustomers => _context.Customers.Where(c => c.IsActive);

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
            return await ActiveCustomers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await ActiveCustomers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetByDocumentAsync(string document)
        {
            return await ActiveCustomers.FirstOrDefaultAsync(c => c.Document == document);
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }
    }
}