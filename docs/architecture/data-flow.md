# 🔄 Fluxo de Dados

<div align="center">

![CQRS](https://img.shields.io/badge/Pattern-CQRS-blue?style=for-the-badge)
![Flow](https://img.shields.io/badge/Data_Flow-Unidirectional-green?style=for-the-badge)

</div>

## 📋 Índice

- [🎯 Visão Geral](#-visão-geral)
- [✍️ Fluxo de Escrita (Commands)](#️-fluxo-de-escrita-commands)
- [📖 Fluxo de Leitura (Queries)](#-fluxo-de-leitura-queries)
- [🔍 Fluxo de Validação](#-fluxo-de-validação)
- [⚡ Fluxo de Eventos](#-fluxo-de-eventos)
- [🔥 Tratamento de Erros](#-tratamento-de-erros)
- [📊 Exemplos Práticos](#-exemplos-práticos)

## 🎯 Visão Geral

O VibraTicket implementa **CQRS (Command Query Responsibility Segregation)** para separar operações de leitura e escrita, proporcionando:

- ✅ **Otimização independente** de leituras e escritas
- ✅ **Modelos específicos** para cada operação
- ✅ **Escalabilidade** diferenciada
- ✅ **Auditoria** simplificada
- ✅ **Performance** otimizada

```mermaid
graph LR
    subgraph "Cliente"
        REQ[HTTP Request]
    end

    subgraph "API Gateway"
        ROUTE{Route}
    end

    subgraph "Command Side"
        CMD[Commands]
        UC[Use Cases]
        DOM[Domain Model]
        WDB[(Write DB)]
    end

    subgraph "Query Side"
        QRY[Queries]
        QH[Query Handlers]
        PROJ[Projections]
        RDB[(Read DB)]
    end

    REQ --> ROUTE
    ROUTE -->|Write| CMD
    ROUTE -->|Read| QRY
    CMD --> UC
    UC --> DOM
    DOM --> WDB
    QRY --> QH
    QH --> PROJ
    PROJ --> RDB

    classDef client fill:#e3f2fd,stroke:#1565c0
    classDef command fill:#fff3e0,stroke:#f57c00
    classDef query fill:#e8f5e9,stroke:#388e3c

    class REQ client
    class CMD,UC,DOM,WDB command
    class QRY,QH,PROJ,RDB query
```

## ✍️ Fluxo de Escrita (Commands)

### Sequência Detalhada

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant Validator
    participant UseCase
    participant Domain
    participant Repository
    participant UnitOfWork
    participant EventBus
    participant Database

    Client->>Controller: POST /api/orders
    Controller->>Controller: Deserialize Request
    Controller->>Validator: Validate(CreateOrderRequest)
    
    alt Validation Failed
        Validator-->>Controller: ValidationError
        Controller-->>Client: 400 Bad Request
    else Validation Success
        Validator-->>Controller: Valid
        Controller->>UseCase: ExecuteAsync(request)
        UseCase->>Domain: Create Order Entity
        Domain->>Domain: Apply Business Rules
        Domain->>Domain: Raise Domain Events
        UseCase->>Repository: AddAsync(order)
        Repository->>Database: BEGIN TRANSACTION
        Repository->>Database: INSERT Order
        Repository->>Database: INSERT OrderItems
        UseCase->>UnitOfWork: CommitAsync()
        UnitOfWork->>Database: COMMIT
        UnitOfWork->>EventBus: Publish Events
        UseCase-->>Controller: OrderCreatedResponse
        Controller-->>Client: 201 Created
    end
```

### Exemplo de Command Flow

```csharp
// 1️⃣ Controller recebe a requisição
[HttpPost]
public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
{
    var result = await _createOrderUseCase.ExecuteAsync(request);
    return Created($"/api/orders/{result.Id}", result);
}

// 2️⃣ Use Case orquestra a operação
public async Task<OrderResponse> ExecuteAsync(CreateOrderRequest request)
{
    // Validação
    await _validator.ValidateAndThrowAsync(request);
    
    // Buscar dados necessários
    var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
    if (customer == null)
        throw new NotFoundException("Customer not found");
    
    // Criar entidade aplicando regras de negócio
    var order = customer.PlaceOrder(request.Items.Select(i => 
        new OrderItem(i.ProductId, i.Quantity, i.Price)));
    
    // Persistir
    await _orderRepository.AddAsync(order);
    await _unitOfWork.CommitAsync();
    
    // Retornar response
    return _mapper.Map<OrderResponse>(order);
}

// 3️⃣ Domain aplica regras de negócio
public Order PlaceOrder(IEnumerable<OrderItem> items)
{
    if (Status != CustomerStatus.Active)
        throw new DomainException("Inactive customer cannot place orders");
        
    var order = new Order(this, items);
    
    // Raise domain event
    AddDomainEvent(new OrderPlacedEvent(order.Id, Id, order.Total));
    
    return order;
}
```

## 📖 Fluxo de Leitura (Queries)

### Sequência Otimizada

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant QueryHandler
    participant ReadRepository
    participant Cache
    participant Database

    Client->>Controller: GET /api/events?page=1&size=10
    Controller->>QueryHandler: HandleAsync(GetEventsQuery)
    QueryHandler->>Cache: Check Cache
    
    alt Cache Hit
        Cache-->>QueryHandler: Cached Data
    else Cache Miss
        QueryHandler->>ReadRepository: GetEventsAsync(page, size)
        ReadRepository->>Database: SELECT * FROM EventsView
        Database-->>ReadRepository: Event Data
        ReadRepository-->>QueryHandler: EventDTO[]
        QueryHandler->>Cache: Store in Cache
    end
    
    QueryHandler-->>Controller: PagedResult<EventDTO>
    Controller-->>Client: 200 OK
```

### Exemplo de Query Flow

```csharp
// 1️⃣ Controller
[HttpGet]
public async Task<IActionResult> GetEvents([FromQuery] GetEventsQuery query)
{
    var result = await _mediator.Send(query);
    return Ok(result);
}

// 2️⃣ Query Handler otimizado para leitura
public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, PagedResult<EventDTO>>
{
    private readonly IEventReadRepository _repository;
    private readonly IMemoryCache _cache;

    public async Task<PagedResult<EventDTO>> Handle(
        GetEventsQuery request, 
        CancellationToken cancellationToken)
    {
        var cacheKey = $"events_{request.Page}_{request.PageSize}_{request.Filter}";
        
        if (_cache.TryGetValue(cacheKey, out PagedResult<EventDTO> cached))
            return cached;
        
        var events = await _repository.GetPagedAsync(
            page: request.Page,
            pageSize: request.PageSize,
            filter: request.Filter,
            orderBy: request.OrderBy);
        
        _cache.Set(cacheKey, events, TimeSpan.FromMinutes(5));
        
        return events;
    }
}

// 3️⃣ Read Repository com query otimizada
public async Task<PagedResult<EventDTO>> GetPagedAsync(
    int page, int pageSize, string filter, string orderBy)
{
    var query = _context.Events
        .AsNoTracking()
        .Include(e => e.Venue)
        .Include(e => e.Sectors)
        .Where(e => e.IsActive);
    
    if (!string.IsNullOrEmpty(filter))
        query = query.Where(e => e.Name.Contains(filter));
    
    var total = await query.CountAsync();
    
    var items = await query
        .OrderBy(orderBy ?? "Name")
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(e => new EventDTO
        {
            Id = e.Id,
            Name = e.Name,
            Date = e.Date,
            VenueName = e.Venue.Name,
            TotalCapacity = e.Sectors.Sum(s => s.Capacity),
            AvailableTickets = e.Sectors.Sum(s => s.AvailableTickets)
        })
        .ToListAsync();
    
    return new PagedResult<EventDTO>(items, total, page, pageSize);
}
```

## 🔍 Fluxo de Validação

### Pipeline de Validação em Camadas

```mermaid
graph TD
    subgraph "1. Input Validation"
        IV1[Data Type Check]
        IV2[Required Fields]
        IV3[Format Validation]
        IV4[Range Validation]
    end

    subgraph "2. Business Validation"
        BV1[Business Rules]
        BV2[Cross-field Logic]
        BV3[External Dependencies]
        BV4[State Validation]
    end

    subgraph "3. Domain Validation"
        DV1[Invariants]
        DV2[Aggregate Rules]
        DV3[Entity State]
        DV4[Value Objects]
    end

    subgraph "4. Persistence Validation"
        PV1[Unique Constraints]
        PV2[Foreign Keys]
        PV3[Data Integrity]
    end

    REQ[Request] --> IV1
    IV1 --> IV2 --> IV3 --> IV4
    IV4 --> BV1
    BV1 --> BV2 --> BV3 --> BV4
    BV4 --> DV1
    DV1 --> DV2 --> DV3 --> DV4
    DV4 --> PV1
    PV1 --> PV2 --> PV3
    PV3 --> SUCCESS[Success]

    IV1 -.-> ERR[Validation Error]
    BV1 -.-> ERR
    DV1 -.-> ERR
    PV1 -.-> ERR

    classDef input fill:#e3f2fd,stroke:#1565c0
    classDef business fill:#f3e5f5,stroke:#7b1fa2
    classDef domain fill:#fff3e0,stroke:#f57c00
    classDef persistence fill:#fce4ec,stroke:#c2185b
    classDef error fill:#ffebee,stroke:#c62828

    class IV1,IV2,IV3,IV4 input
    class BV1,BV2,BV3,BV4 business
    class DV1,DV2,DV3,DV4 domain
    class PV1,PV2,PV3 persistence
    class ERR error
```

### Implementação de Validadores

```csharp
// 1️⃣ Input Validation (FluentValidation)
public class CreateEventValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required")
            .MaximumLength(200).WithMessage("Event name too long");
        
        RuleFor(x => x.Date)
            .GreaterThan(DateTime.Now).WithMessage("Event must be in the future");
        
        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("Venue is required");
        
        RuleFor(x => x.Sectors)
            .NotEmpty().WithMessage("At least one sector is required")
            .ForEach(sector => sector.SetValidator(new SectorValidator()));
    }
}

// 2️⃣ Business Validation (Use Case)
public class CreateEventUseCase
{
    public async Task<EventResponse> ExecuteAsync(CreateEventRequest request)
    {
        // Check venue availability
        var isVenueAvailable = await _venueService
            .IsAvailableAsync(request.VenueId, request.Date);
        
        if (!isVenueAvailable)
            throw new BusinessException("Venue is not available on this date");
        
        // Check affiliate permissions
        var affiliate = await _affiliateRepository.GetByIdAsync(request.AffiliateId);
        if (!affiliate.CanCreateEvents())
            throw new BusinessException("Affiliate cannot create events");
        
        // Continue with creation...
    }
}

// 3️⃣ Domain Validation (Entity)
public class Event : Entity
{
    public Event(string name, DateTime date, Venue venue, Affiliate affiliate)
    {
        // Domain invariants
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Event name cannot be empty");
        
        if (date <= DateTime.Now)
            throw new DomainException("Event date must be in the future");
        
        if (venue == null)
            throw new DomainException("Event must have a venue");
        
        if (!venue.IsActive)
            throw new DomainException("Cannot create event in inactive venue");
        
        Name = name;
        Date = date;
        Venue = venue;
        Affiliate = affiliate;
    }
}
```

## ⚡ Fluxo de Eventos

### Domain Events Pipeline

```mermaid
graph LR
    subgraph "Domain Layer"
        ENT[Entity]
        EVT[Domain Event]
        ENT -->|Raises| EVT
    end

    subgraph "Application Layer"
        DISP[Event Dispatcher]
        H1[Handler 1]
        H2[Handler 2]
        H3[Handler 3]
    end

    subgraph "Infrastructure"
        BUS[Event Bus]
        Q1[Queue 1]
        Q2[Queue 2]
        LOG[Event Log]
    end

    EVT --> DISP
    DISP --> H1
    DISP --> H2
    DISP --> H3
    H1 --> BUS
    H2 --> BUS
    BUS --> Q1
    BUS --> Q2
    DISP --> LOG

    classDef domain fill:#fff3e0,stroke:#f57c00
    classDef app fill:#f3e5f5,stroke:#7b1fa2
    classDef infra fill:#fce4ec,stroke:#c2185b

    class ENT,EVT domain
    class DISP,H1,H2,H3 app
    class BUS,Q1,Q2,LOG infra
```

### Implementação de Eventos

```csharp
// 1️⃣ Domain Event
public class OrderPlacedEvent : DomainEvent
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public decimal TotalAmount { get; }
    public DateTime PlacedAt { get; }

    public OrderPlacedEvent(Guid orderId, Guid customerId, decimal totalAmount)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        PlacedAt = DateTime.UtcNow;
    }
}

// 2️⃣ Event Handler
public class OrderPlacedEventHandler : INotificationHandler<OrderPlacedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IInventoryService _inventoryService;
    private readonly IAnalyticsService _analyticsService;

    public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
    {
        // Send confirmation email
        await _emailService.SendOrderConfirmationAsync(
            notification.CustomerId, 
            notification.OrderId);
        
        // Update inventory
        await _inventoryService.ReserveTicketsAsync(notification.OrderId);
        
        // Track analytics
        await _analyticsService.TrackOrderAsync(new OrderAnalytics
        {
            OrderId = notification.OrderId,
            Amount = notification.TotalAmount,
            Timestamp = notification.PlacedAt
        });
    }
}
```

## 🔥 Tratamento de Erros

### Fluxo de Exceções

```mermaid
graph TD
    subgraph "Exception Sources"
        VAL[Validation Error]
        BUS[Business Error]
        DOM[Domain Error]
        INF[Infrastructure Error]
    end

    subgraph "Exception Handler"
        HANDLER[Global Exception Handler]
        MAPPER[Error Mapper]
        LOGGER[Error Logger]
    end

    subgraph "Response"
        400[400 Bad Request]
        404[404 Not Found]
        409[409 Conflict]
        500[500 Internal Error]
    end

    VAL --> HANDLER
    BUS --> HANDLER
    DOM --> HANDLER
    INF --> HANDLER

    HANDLER --> MAPPER
    HANDLER --> LOGGER
    
    MAPPER --> 400
    MAPPER --> 404
    MAPPER --> 409
    MAPPER --> 500

    classDef error fill:#ffebee,stroke:#c62828
    classDef handler fill:#e8eaf6,stroke:#3f51b5
    classDef response fill:#f5f5f5,stroke:#616161

    class VAL,BUS,DOM,INF error
    class HANDLER,MAPPER,LOGGER handler
    class 400,404,409,500 response
```

### Global Exception Handler

```csharp
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred processing the request");

        var response = exception switch
        {
            ValidationException valEx => new ErrorResponse(
                StatusCodes.Status400BadRequest,
                "Validation failed",
                valEx.Errors.Select(e => new ErrorDetail(e.PropertyName, e.ErrorMessage))),
            
            NotFoundException notFound => new ErrorResponse(
                StatusCodes.Status404NotFound,
                notFound.Message),
            
            DomainException domain => new ErrorResponse(
                StatusCodes.Status409Conflict,
                domain.Message),
            
            _ => new ErrorResponse(
                StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request")
        };

        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
```

## 📊 Exemplos Práticos

### Fluxo Completo: Compra de Ingresso

```mermaid
stateDiagram-v2
    [*] --> SelectEvent: Customer browses events
    SelectEvent --> SelectSector: Choose event
    SelectSector --> SelectQuantity: Choose sector
    SelectQuantity --> AddToCart: Select tickets
    AddToCart --> Review: View cart
    Review --> Checkout: Proceed
    Checkout --> Payment: Enter payment
    Payment --> Processing: Process payment
    
    Processing --> Success: Payment approved
    Processing --> Failed: Payment failed
    
    Success --> TicketGeneration: Generate tickets
    TicketGeneration --> EmailConfirmation: Send confirmation
    EmailConfirmation --> [*]: Complete
    
    Failed --> Checkout: Retry payment
    
    Review --> SelectEvent: Continue shopping
    SelectQuantity --> SelectEvent: Back
```

### Código do Fluxo

```csharp
// 1️⃣ Select Event (Query)
public async Task<IEnumerable<EventListDTO>> GetAvailableEvents()
{
    return await _context.Events
        .Where(e => e.Date > DateTime.Now && e.HasAvailableTickets)
        .Select(e => new EventListDTO
        {
            Id = e.Id,
            Name = e.Name,
            Date = e.Date,
            Venue = e.Venue.Name,
            LowestPrice = e.Sectors.Min(s => s.Price),
            AvailableTickets = e.Sectors.Sum(s => s.AvailableTickets)
        })
        .ToListAsync();
}

// 2️⃣ Create Order (Command)
public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
{
    // Begin transaction
    using var transaction = await _unitOfWork.BeginTransactionAsync();
    
    try
    {
        // Create order
        var order = new Order(request.CustomerId);
        
        // Add items and validate availability
        foreach (var item in request.Items)
        {
            var sector = await _sectorRepository.GetByIdAsync(item.SectorId);
            if (!sector.HasAvailableTickets(item.Quantity))
                throw new BusinessException($"Not enough tickets in sector {sector.Name}");
            
            order.AddItem(sector, item.Quantity);
            sector.ReserveTickets(item.Quantity);
        }
        
        // Save order
        await _orderRepository.AddAsync(order);
        await _unitOfWork.CommitAsync();
        
        // Process payment
        var paymentResult = await _paymentService.ProcessAsync(order);
        
        if (paymentResult.IsSuccess)
        {
            order.ConfirmPayment(paymentResult.TransactionId);
            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();
            
            // Publish events
            await _eventBus.PublishAsync(new OrderCompletedEvent(order));
        }
        else
        {
            await transaction.RollbackAsync();
            throw new PaymentException(paymentResult.ErrorMessage);
        }
        
        return _mapper.Map<OrderResponse>(order);
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

---

<div align="center">

[← Camadas](./layers.md) | [Próximo: Modelo de Domínio →](./domain-model.md)

</div>