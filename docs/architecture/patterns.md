# 🔧 Padrões e Práticas

<div align="center">

![Patterns](https://img.shields.io/badge/Design_Patterns-12-blue?style=for-the-badge)
![Best Practices](https://img.shields.io/badge/Best_Practices-Implemented-green?style=for-the-badge)

</div>

## 📑 Índice

- [🎨 Padrões de Design](#-padrões-de-design)
- [📐 Padrões Arquiteturais](#-padrões-arquiteturais)
- [💻 Padrões de Código](#-padrões-de-código)
- [🔄 Padrões de Integração](#-padrões-de-integração)
- [🧪 Padrões de Teste](#-padrões-de-teste)
- [📊 Padrões de Performance](#-padrões-de-performance)

## 🎨 Padrões de Design

### Repository Pattern

**Objetivo**: Abstrair o acesso a dados e permitir troca de implementação

```csharp
// Interface no Domain
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByDocumentAsync(string document);
    Task<Customer?> GetByEmailAsync(string email);
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    void Remove(Customer customer);
}

// Implementação no Infrastructure
public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
    
    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    // Outras implementações...
}
```

### Unit of Work Pattern

**Objetivo**: Gerenciar transações e garantir consistência

```csharp
public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _currentTransaction;
    
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }
}
```

### Specification Pattern

**Objetivo**: Encapsular regras de negócio complexas e reutilizáveis

```csharp
public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();
    
    public bool IsSatisfiedBy(T entity)
    {
        return ToExpression().Compile().Invoke(entity);
    }
    
    public Specification<T> And(Specification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }
    
    public Specification<T> Or(Specification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }
}

// Exemplo de uso
public class ActiveCustomerSpecification : Specification<Customer>
{
    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => customer.Status == CustomerStatus.Active;
    }
}

public class PremiumCustomerSpecification : Specification<Customer>
{
    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => customer.Orders.Count > 10 || 
                          customer.Orders.Sum(o => o.Total) > 5000;
    }
}

// Uso combinado
var premiumActiveSpec = new ActiveCustomerSpecification()
    .And(new PremiumCustomerSpecification());
```

### Factory Pattern

**Objetivo**: Criar objetos complexos com lógica de construção

```csharp
public interface ITicketFactory
{
    Ticket CreateTicket(Order order, Sector sector);
    IEnumerable<Ticket> CreateTickets(Order order, OrderItem item);
}

public class TicketFactory : ITicketFactory
{
    private readonly ITicketCodeGenerator _codeGenerator;
    
    public Ticket CreateTicket(Order order, Sector sector)
    {
        var code = _codeGenerator.Generate(order, sector);
        
        return new Ticket(
            code: code,
            order: order,
            sector: sector,
            generatedAt: DateTime.UtcNow
        );
    }
    
    public IEnumerable<Ticket> CreateTickets(Order order, OrderItem item)
    {
        for (int i = 0; i < item.Quantity; i++)
        {
            yield return CreateTicket(order, item.Sector);
        }
    }
}
```

### Strategy Pattern

**Objetivo**: Permitir múltiplas implementações de algoritmos

```csharp
public interface IPriceCalculationStrategy
{
    decimal Calculate(Order order, Customer customer);
}

public class RegularPriceStrategy : IPriceCalculationStrategy
{
    public decimal Calculate(Order order, Customer customer)
    {
        return order.Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}

public class CorporatePriceStrategy : IPriceCalculationStrategy
{
    private const decimal CorporateDiscount = 0.10m;
    
    public decimal Calculate(Order order, Customer customer)
    {
        var subtotal = order.Items.Sum(i => i.Quantity * i.UnitPrice);
        return subtotal * (1 - CorporateDiscount);
    }
}

public class VIPPriceStrategy : IPriceCalculationStrategy
{
    private const decimal VIPDiscount = 0.20m;
    
    public decimal Calculate(Order order, Customer customer)
    {
        var subtotal = order.Items.Sum(i => i.Quantity * i.UnitPrice);
        var discount = customer.IsBirthday ? 0.30m : VIPDiscount;
        return subtotal * (1 - discount);
    }
}

// Context
public class PriceCalculator
{
    private readonly Dictionary<CustomerType, IPriceCalculationStrategy> _strategies;
    
    public decimal CalculatePrice(Order order, Customer customer)
    {
        var strategy = _strategies[customer.Type];
        return strategy.Calculate(order, customer);
    }
}
```

### Observer Pattern (Domain Events)

**Objetivo**: Notificar mudanças de estado sem acoplamento

```csharp
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

// Domain Event
public class OrderPlacedEvent : IDomainEvent
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public decimal Total { get; }
    public DateTime OccurredAt { get; }
}

// Event Handler
public class OrderPlacedEventHandler : INotificationHandler<OrderPlacedEvent>
{
    public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
    {
        await _emailService.SendOrderConfirmation(notification.OrderId);
        await _inventoryService.ReserveItems(notification.OrderId);
        await _analyticsService.TrackPurchase(notification);
    }
}
```

## 📐 Padrões Arquiteturais

### CQRS (Command Query Responsibility Segregation)

**Commands (Write Side)**
```csharp
public class CreateEventCommand : IRequest<EventResponse>
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public Guid VenueId { get; set; }
    public List<CreateSectorDto> Sectors { get; set; }
}

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
    public async Task<EventResponse> Handle(CreateEventCommand request, CancellationToken ct)
    {
        var venue = await _venueRepository.GetByIdAsync(request.VenueId);
        var event = new Event(request.Name, request.Date, venue);
        
        foreach (var sectorDto in request.Sectors)
        {
            event.AddSector(sectorDto.Name, sectorDto.Price, sectorDto.Capacity);
        }
        
        await _eventRepository.AddAsync(event);
        await _unitOfWork.CommitAsync(ct);
        
        return _mapper.Map<EventResponse>(event);
    }
}
```

**Queries (Read Side)**
```csharp
public class GetEventsQuery : IRequest<PagedResult<EventListDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
    public EventStatus? Status { get; set; }
}

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, PagedResult<EventListDto>>
{
    public async Task<PagedResult<EventListDto>> Handle(GetEventsQuery request, CancellationToken ct)
    {
        var query = _context.Events
            .AsNoTracking()
            .Where(e => request.Status == null || e.Status == request.Status)
            .Where(e => string.IsNullOrEmpty(request.Search) || 
                       e.Name.Contains(request.Search));
        
        var total = await query.CountAsync(ct);
        
        var events = await query
            .OrderBy(e => e.Date)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<EventListDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        
        return new PagedResult<EventListDto>(events, total, request.Page, request.PageSize);
    }
}
```

### Mediator Pattern

**Objetivo**: Desacoplar componentes através de um mediador central

```csharp
// Controller simplificado
[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpPost]
    public async Task<ActionResult<EventResponse>> Create(CreateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/events/{result.Id}", result);
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<EventListDto>>> List([FromQuery] GetEventsQuery query)
    {
        return await _mediator.Send(query);
    }
}
```

## 💻 Padrões de Código

### Guard Clauses

```csharp
public class Event
{
    public Event(string name, DateTime date, Venue venue)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.OutOfSQLDateRange(date, nameof(date));
        Guard.Against.Null(venue, nameof(venue));
        Guard.Against.NotInFuture(date, nameof(date), "Event must be in the future");
        
        Name = name;
        Date = date;
        Venue = venue;
    }
}

public static class Guard
{
    public static class Against
    {
        public static void Null<T>(T value, string parameterName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }
        
        public static void NullOrWhiteSpace(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace", parameterName);
        }
        
        public static void NotInFuture(DateTime value, string parameterName, string message)
        {
            if (value <= DateTime.UtcNow)
                throw new ArgumentException(message, parameterName);
        }
    }
}
```

### Result Pattern

```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    
    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}

// Uso
public async Task<Result<Order>> PlaceOrderAsync(PlaceOrderCommand command)
{
    var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
    if (customer == null)
        return Result<Order>.Failure("Customer not found");
    
    if (!customer.CanPlaceOrder())
        return Result<Order>.Failure("Customer cannot place orders");
    
    var order = customer.PlaceOrder(command.Items);
    await _orderRepository.AddAsync(order);
    await _unitOfWork.CommitAsync();
    
    return Result<Order>.Success(order);
}
```

### Options Pattern

```csharp
public class JwtOptions
{
    public const string Section = "Jwt";
    
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}

// Configuration
services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Section));

// Usage
public class JwtService
{
    private readonly JwtOptions _options;
    
    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
}
```

## 🔄 Padrões de Integração

### Gateway Pattern

```csharp
public interface IPaymentGateway
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<PaymentStatus> GetStatusAsync(string transactionId);
    Task<RefundResult> RefundAsync(string transactionId, decimal amount);
}

public class StripePaymentGateway : IPaymentGateway
{
    private readonly HttpClient _httpClient;
    private readonly StripeOptions _options;
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var stripeRequest = MapToStripeRequest(request);
        var response = await _httpClient.PostAsJsonAsync("charges", stripeRequest);
        
        if (response.IsSuccessStatusCode)
        {
            var stripeResponse = await response.Content.ReadFromJsonAsync<StripeChargeResponse>();
            return MapToPaymentResult(stripeResponse);
        }
        
        return PaymentResult.Failure("Payment processing failed");
    }
}
```

### Retry Pattern with Polly

```csharp
public class ResilientEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
    
    public ResilientEmailService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    var logger = context.Values["logger"] as ILogger;
                    logger?.LogWarning(
                        "Retry {RetryCount} after {TimeSpan}s", 
                        retryCount, 
                        timespan.TotalSeconds);
                });
    }
    
    public async Task SendAsync(EmailMessage message)
    {
        var response = await _retryPolicy.ExecuteAsync(async () =>
            await _httpClient.PostAsJsonAsync("send", message)
        );
        
        response.EnsureSuccessStatusCode();
    }
}
```

## 🧪 Padrões de Teste

### Builder Pattern para Testes

```csharp
public class CustomerBuilder
{
    private Name _name = new("John", "Doe");
    private Email _email = new("john@test.com");
    private Document _document = new("12345678901");
    private Phone _phone = new("+5511999999999");
    private CustomerType _type = CustomerType.Individual;
    private CustomerStatus _status = CustomerStatus.Active;
    
    public CustomerBuilder WithName(string firstName, string lastName)
    {
        _name = new Name(firstName, lastName);
        return this;
    }
    
    public CustomerBuilder WithEmail(string email)
    {
        _email = new Email(email);
        return this;
    }
    
    public CustomerBuilder AsInactive()
    {
        _status = CustomerStatus.Inactive;
        return this;
    }
    
    public CustomerBuilder AsCorporate()
    {
        _type = CustomerType.Corporate;
        return this;
    }
    
    public Customer Build()
    {
        return new Customer(_name, _email, _document, _phone, _type, _status);
    }
}

// Uso em testes
[Fact]
public void Customer_ShouldNotPlaceOrder_WhenInactive()
{
    // Arrange
    var customer = new CustomerBuilder()
        .AsInactive()
        .Build();
    
    // Act & Assert
    Assert.Throws<DomainException>(() => customer.PlaceOrder(items));
}
```

### Object Mother Pattern

```csharp
public static class ObjectMother
{
    public static class Customers
    {
        public static Customer ActiveIndividual() =>
            new CustomerBuilder().Build();
        
        public static Customer InactiveIndividual() =>
            new CustomerBuilder().AsInactive().Build();
        
        public static Customer ActiveCorporate() =>
            new CustomerBuilder().AsCorporate().Build();
        
        public static Customer Premium() =>
            new CustomerBuilder()
                .WithName("Premium", "Customer")
                .Build();
    }
    
    public static class Events
    {
        public static Event RockFestival() =>
            new EventBuilder()
                .WithName("Rock Festival 2024")
                .WithDate(DateTime.Now.AddMonths(3))
                .WithVenue(Venues.Stadium())
                .Build();
        
        public static Event SoldOutConcert() =>
            new EventBuilder()
                .WithName("Sold Out Concert")
                .WithNoAvailableTickets()
                .Build();
    }
}
```

## 📊 Padrões de Performance

### Lazy Loading Pattern

```csharp
public class Event
{
    private readonly Lazy<ICollection<Sector>> _sectors;
    private readonly Lazy<int> _totalCapacity;
    
    public Event()
    {
        _sectors = new Lazy<ICollection<Sector>>(() => LoadSectors());
        _totalCapacity = new Lazy<int>(() => CalculateTotalCapacity());
    }
    
    public ICollection<Sector> Sectors => _sectors.Value;
    public int TotalCapacity => _totalCapacity.Value;
    
    private ICollection<Sector> LoadSectors()
    {
        // Load sectors from repository
        return _sectorRepository.GetByEventId(Id);
    }
    
    private int CalculateTotalCapacity()
    {
        return Sectors.Sum(s => s.Capacity);
    }
}
```

### Cache-Aside Pattern

```csharp
public class CachedEventRepository : IEventRepository
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(15);
    
    public async Task<Event?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"event_{id}";
        
        if (_cache.TryGetValue(cacheKey, out Event cachedEvent))
            return cachedEvent;
        
        var event = await _repository.GetByIdAsync(id);
        
        if (event != null)
        {
            _cache.Set(cacheKey, event, _cacheExpiration);
        }
        
        return event;
    }
    
    public async Task UpdateAsync(Event event)
    {
        await _repository.UpdateAsync(event);
        
        // Invalidate cache
        _cache.Remove($"event_{event.Id}");
    }
}
```

### Bulk Operations Pattern

```csharp
public class BulkTicketGenerator
{
    public async Task<IEnumerable<Ticket>> GenerateTicketsAsync(
        Order order, 
        IEnumerable<OrderItem> items)
    {
        var tickets = new List<Ticket>();
        var bulkData = new List<TicketEntity>();
        
        foreach (var item in items)
        {
            for (int i = 0; i < item.Quantity; i++)
            {
                var ticket = new Ticket(
                    code: GenerateCode(),
                    order: order,
                    sector: item.Sector
                );
                
                tickets.Add(ticket);
                bulkData.Add(MapToEntity(ticket));
            }
        }
        
        // Bulk insert instead of individual inserts
        await _context.BulkInsertAsync(bulkData);
        
        return tickets;
    }
}
```

---

<div align="center">

[← Decisões Arquiteturais](./decisions.md) | [Início →](./README.md)

</div>