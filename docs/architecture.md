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
## Diagrama de Endpoints da API
graph TD
    subgraph API
        CREATE[POST /customers] --> CONTROLLER[CustomersController]
        UPDATE[PUT /customers/{id}] --> CONTROLLER
        GETBYID[GET /customers/{id}] --> CONTROLLER
        GETBYDOC[GET /customers/by-document/{document}] --> CONTROLLER
        GETBYEMAIL[GET /customers/by-email/{email}] --> CONTROLLER
    end

    CONTROLLER --> USECASE[UseCases]
    USECASE --> REPOSITORY[Repositories]
    REPOSITORY --> DB[(Database)]
## Diagrama de Relacionamentos entre Entidades
graph TD
    Customer -->|1..*| Order
    Order -->|*..1| Customer
    Order -->|*..1| Payment
    Order -->|*..*| Ticket
    Event -->|1..*| Sector
    Sector -->|1..*| Ticket
    Event -->|*..1| Venue
    Affiliate -->|1..*| Event
## Fluxo de Dados
graph LR
    HTTP[HTTP Request] --> CONTROLLER[Controller]
    CONTROLLER --> USECASE[UseCase/Query]
    USECASE --> REPOSITORY[Repository]
    REPOSITORY --> DB[(Database)]
    DB --> REPOSITORY
    REPOSITORY --> DTO[DTO/Response]
    DTO --> CONTROLLER
    CONTROLLER --> HTTPRESPONSE[HTTP Response]
---

> Para mais detalhes, consulte o código-fonte e o README principal.