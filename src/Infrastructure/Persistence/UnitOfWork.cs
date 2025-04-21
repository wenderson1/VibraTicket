using Application.Interfaces;
using Application.Interfaces.Repository;
using Infrastructure.Repositories;

namespace Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IVenueRepository? _venueRepository;
    
    public IVenueRepository Venues => _venueRepository ??= new VenueRepository(context);
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}