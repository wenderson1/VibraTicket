# Arquitetura do VibraTicket

## Diagrama Geral
graph TD
    API[API Layer]
    APP[Application Layer]
    DOMAIN[Domain Layer]
    INFRA[Infrastructure Layer]

    API --> APP
    APP --> DOMAIN
    INFRA --> APP
    INFRA --> DOMAIN

    subgraph API Layer
        CONTROLLERS[Controllers]
        MIDDLEWARE[Middleware]
        DI[Dependency Injection]
    end

    subgraph Application Layer
        USECASES[UseCases]
        QUERIES[Queries]
        VALIDATORS[Validators]
        DTOs[DTOs]
        INTERFACES[Interfaces]
    end

    subgraph Domain Layer
        ENTITIES[Entities]
        VALUEOBJECTS[Value Objects]
        REPOINTERFACES[Repository Interfaces]
    end

    subgraph Infrastructure Layer
        DB[Database]
        REPOSITORIES[Repositories]
        UOW[Unit of Work]
        CONFIGS[Entity Configurations]
    end
## Diagrama das Entidades
graph TD
    Customer -->|1..*| Order
    Order -->|*..1| Customer
    Order -->|*..1| Payment
    Order -->|*..*| Ticket
    Event -->|1..*| Sector
    Sector -->|1..*| Ticket
    Event -->|*..1| Venue
    Affiliate -->|1..*| Event
## Fluxos

- **Request:** Controller ? UseCase/Query ? Repository ? Database
- **Response:** Database ? Repository ? Query/DTO ? Controller ? HTTP Response
- **Validação:** Input ? Validator ? UseCase ? Result

## Camadas

### API
- Controllers, Middleware, DI, Swagger

### Application
- UseCases, Queries, Validators, DTOs, Interfaces

### Domain
- Entities, Value Objects, Repository Interfaces

### Infrastructure
- DbContext, Repositories, Unit of Work, Configurações

## Padrões de Projeto

- CQRS (UseCases/Queries)
- Repository & Unit of Work
- Dependency Injection
- Result Pattern
- Validator Pattern

---

> Para detalhes de entidades, regras e exemplos, consulte o código-fonte e o README principal.