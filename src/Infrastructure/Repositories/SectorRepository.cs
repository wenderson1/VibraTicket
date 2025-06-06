using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SectorRepository(DbContext context) : ISectorRepository
    {
        public Task AddAsync(Sector sector)
        {
            throw new NotImplementedException();
        }

        public void Delete(Sector sector)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sector>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Sector?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Sector sector)
        {
            throw new NotImplementedException();
        }
    }
}
