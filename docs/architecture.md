# Diagrama Técnico Detalhado

```mermaid
graph TB
    subgraph "API Layer"
        CONT[Controllers] --> MWARE[Middleware]
        CONT --> DI[Dependency Injection]
        CONT --> FILTER[Filters]
    end

    subgraph "Application Layer"
        UC[Use Cases] --> VAL[Validators]
        UC --> PORT[Interfaces/Ports]
        QUERY[Queries] --> PORT
        UC --> COMMON[Commons]
        QUERY --> COMMON
    end

    subgraph "Domain Layer"
        ENT[Entities] --> VO[Value Objects]
        ENT --> RULES[Domain Rules]
        ENT --> EVENTS[Domain Events]
    end

    subgraph "Infrastructure Layer"
        DB[(Database)] --> REPOS[Repositories]
        REPOS --> UOW[Unit of Work]
        CONF[Entity Configurations] --> DB
    end

    %% Cross-layer dependencies
    CONT --> UC
    CONT --> QUERY
    UC --> ENT
    QUERY --> ENT
    REPOS --> ENT
    UOW --> PORT

    %% Entity relationships
    subgraph "Domain Entities"
        CUSTOMER[Customer] --> ORDER[Order]
        EVENT[Event] --> SECTOR[Sector]
        SECTOR --> TICKET[Ticket]
        ORDER --> TICKET
        ORDER --> PAYMENT[Payment]
        EVENT --> VENUE[Venue]
        AFFILIATE[Affiliate] --> EVENT
    end

    %% Use Cases flow
    subgraph "Business Flow"
        CREATE[Create] --> READ[Read]
        READ --> UPDATE[Update]
        UPDATE --> DELETE[Delete]
        CREATE --> VALIDATE[Validate]
        UPDATE --> VALIDATE
    end

    style API fill:#f9f,stroke:#333,stroke-width:2px
    style Application fill:#bbf,stroke:#333,stroke-width:2px
    style Domain fill:#bfb,stroke:#333,stroke-width:2px
    style Infrastructure fill:#fbb,stroke:#333,stroke-width:2px
```

## Detalhes do Fluxo de Dados

1. **Request Flow**
   ```
   HTTP Request ? Controller ? Use Case ? Repository ? Database
   ```

2. **Response Flow**
   ```
   Database ? Repository ? Query ? DTO ? Controller ? HTTP Response
   ```

3. **Validation Flow**
   ```
   Input ? Validator ? Use Case ? Business Rules ? Result
   ```

## Estrutura de Camadas

### API Layer
- Controllers
- Filters
- Middleware
- DI Container
- Error Handling

### Application Layer
- Use Cases
- Queries
- Validators
- DTOs
- Interfaces
- Common Utils

### Domain Layer
- Entities
- Value Objects
- Domain Rules
- Events
- Interfaces

### Infrastructure Layer
- DbContext
- Repositories
- Configurations
- Unit of Work
- External Services

## Design Patterns Utilizados

1. **CQRS Pattern**
   - Commands (Use Cases)
   - Queries

2. **Repository Pattern**
   - Generic Repository
   - Specific Repositories

3. **Unit of Work**
   - Transaction Management
   - Change Tracking

4. **Factory Pattern**
   - Entity Creation
   - DTO Mapping

5. **Validator Pattern**
   - Input Validation
   - Business Rules

6. **Result Pattern**
   - Error Handling
   - Success/Failure