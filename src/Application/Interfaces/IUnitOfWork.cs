using Application.Interfaces.Repository;

namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVenueRepository Venues { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
}