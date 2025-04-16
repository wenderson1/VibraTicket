using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IVenueRepository
    {
        Task<Venue?> GetByIdAsync(int id); // Retorna Venue ou null se não encontrado
        Task<IEnumerable<Venue>> GetAllAsync(); // Retorna todos os Venues
        Task AddAsync(Venue venue); // Adiciona um novo Venue
        void Update(Venue venue); // Atualiza um Venue existente
        void Delete(Venue venue); // Remove um Venue (poderia ser por ID também)
    }
}
