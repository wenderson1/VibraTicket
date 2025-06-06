using Application.Interfaces;
using Application.Interfaces.Repository;
using Infrastructure.Repositories;

namespace Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IVenueRepository? _venueRepository;
    private ISectorRepository? _sectorRepository;
    private IAffiliateRepository? _affiliateRepository;
    
    public IVenueRepository Venues => _venueRepository ??= new VenueRepository(context);
    public ISectorRepository Sectors => _sectorRepository ??= new SectorRepository(context);
    public IAffiliateRepository Affiliates => _affiliateRepository ??= new AffiliateRepository(context);

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