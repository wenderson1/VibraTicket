using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly AppDbContext _context;

        public VenueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Venue?> GetByIdAsync(int id)
        {
            // FindAsync é otimizado para buscar por chave primária
            // Ele ignora filtros globais por padrão, o que geralmente é ok para busca por ID.
            // Se Venue tivesse IsActive e você quisesse respeitar o filtro aqui:
            // return await _context.Venues.FirstOrDefaultAsync(v => v.Id == id);
            return await _context.Venues.FindAsync(id);
        }

        public async Task<IEnumerable<Venue>> GetAllAsync()
        {
            // ToListAsync aplicará o filtro global se Venue tivesse um (atualmente não tem)
            return await _context.Venues.ToListAsync();
            // Se precisasse ignorar filtros:
            // return await _context.Venues.IgnoreQueryFilters().ToListAsync();
        }

        public async Task AddAsync(Venue venue)
        {
            await _context.Venues.AddAsync(venue);
            // Nota: SaveChangesAsync será chamado em outro lugar (ex: Unit of Work ou Handler)
        }

        public void Update(Venue venue)
        {
            // Informa ao EF Core que a entidade foi modificada.
            _context.Entry(venue).State = EntityState.Modified;
            // Ou, se você buscou a entidade primeiro:
            // var existingVenue = await _context.Venues.FindAsync(venue.Id);
            // if (existingVenue != null) { /* Mapear propriedades */ }
            // Nota: SaveChangesAsync será chamado em outro lugar
        }

        public void Delete(Venue venue)
        {
            _context.Venues.Remove(venue);
        }

        // Implementação de exemplo para FindAsync (se adicionado à interface)
        // public async Task<IEnumerable<Venue>> FindAsync(Expression<Func<Venue, bool>> predicate)
        // {
        //     return await _context.Venues.Where(predicate).ToListAsync();
        // }
    }
}