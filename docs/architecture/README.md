# Arquitetura do VibraTicket

Este diret�rio cont�m a documenta��o detalhada da arquitetura do sistema, dividida em se��es espec�ficas:

- [Vis�o Geral das Camadas](layers.md)
- [Endpoints da API](endpoints.md)
- [Fluxo de Dados](data-flow.md)
- [Modelo de Dom�nio](domain-model.md)

## Stack Tecnol�gico

```mermaid
graph TB
    subgraph Backend
        NET[.NET 8]
        EF[Entity Framework Core]
        SQL[SQL Server]
        SWAGGER[Swagger/OpenAPI]
    end

    subgraph Arquitetura
        CLEAN[Clean Architecture]
        DDD[Domain-Driven Design]
        CQRS[CQRS Pattern]
    end

    subgraph Valida��o & Logging
        FLUENT[FluentValidation]
        LOGGING[Structured Logging]
    end

    NET --> EF
    EF --> SQL
    NET --> SWAGGER
    NET --> CLEAN
    CLEAN --> DDD
    CLEAN --> CQRS
    NET --> FLUENT
    NET --> LOGGING
```