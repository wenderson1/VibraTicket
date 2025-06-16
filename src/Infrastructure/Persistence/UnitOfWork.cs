using Application.Interfaces;
using Application.Interfaces.Repository;
using Infrastructure.Repositories;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IVenueRepository? _venueRepository;
    private ISectorRepository? _sectorRepository;
    private IAffiliateRepository? _affiliateRepository;
    private ICustomerRepository? _customerRepository;
    private IEventRepository? _eventRepository;
    private IOrderRepository? _orderRepository;
    private IPaymentRepository? _paymentRepository;
    private ITicketRepository? _ticketRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IVenueRepository Venues => _venueRepository ??= new VenueRepository(_context);
    public ISectorRepository Sectors => _sectorRepository ??= new SectorRepository(_context);
    public IAffiliateRepository Affiliates => _affiliateRepository ??= new AffiliateRepository(_context);
    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
    public IEventRepository Events => _eventRepository ??= new EventRepository(_context);
    public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
    public IPaymentRepository Payments => _paymentRepository ??= new PaymentRepository(_context);
    public ITicketRepository Tickets => _ticketRepository ??= new TicketRepository(_context);

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