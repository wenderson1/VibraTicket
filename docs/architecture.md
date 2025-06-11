# Arquitetura do VibraTicket

## Diagrama Geral de Camadas
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