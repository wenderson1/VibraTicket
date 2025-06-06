using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ISectorRepository
    {
        Task<Sector?> GetByIdAsync(int id); // Retorna Sector ou null se não encontrado
        Task<IEnumerable<Sector>> GetAllAsync(); // Retorna todos os Sectors
        Task AddAsync(Sector sector); // Adiciona um novo Sector
        void Update(Sector sector); // Atualiza um Sector existente
        void Delete(Sector sector); // Remove um Sector
    }
}
