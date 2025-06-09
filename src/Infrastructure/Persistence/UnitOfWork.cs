using Application.Interfaces;
using Application.Interfaces.Repository;
using Infrastructure.Repositories;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Affiliates = new AffiliateRepository(context);
        Customers = new CustomerRepository(context);
        Events = new EventRepository(context);
        Orders = new OrderRepository(context);
        Payments = new PaymentRepository(context);
        Sectors = new SectorRepository(context);
        Venues = new VenueRepository(context);
    }

    public IAffiliateRepository Affiliates { get; }
    public ICustomerRepository Customers { get; }
    public IEventRepository Events { get; }
    public IOrderRepository Orders { get; }
    public IPaymentRepository Payments { get; }
    public ISectorRepository Sectors { get; }
    public IVenueRepository Venues { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}