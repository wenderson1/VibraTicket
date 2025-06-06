using Application.Interfaces.Repository;

namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVenueRepository Venues { get; }
    ISectorRepository Sectors { get; }
    IAffiliateRepository Affiliates { get; } // Adicionado para suportar Affiliate
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}