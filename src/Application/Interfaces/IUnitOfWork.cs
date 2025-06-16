using Application.Interfaces.Repository;

namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVenueRepository Venues { get; }
    ISectorRepository Sectors { get; }
    IAffiliateRepository Affiliates { get; } // Adicionado para suportar Affiliate
    ICustomerRepository Customers { get; }
    IEventRepository Events { get; }
    IOrderRepository Orders { get; }
    IPaymentRepository Payments { get; }
    ITicketRepository Tickets { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}