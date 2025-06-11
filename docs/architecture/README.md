# Arquitetura do VibraTicket

Este diretório contém a documentação detalhada da arquitetura do sistema, dividida em seções específicas:

- [Visão Geral das Camadas](layers.md)
- [Endpoints da API](endpoints.md)
- [Fluxo de Dados](data-flow.md)
- [Modelo de Domínio](domain-model.md)

## Stack Tecnológico

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

    subgraph Validação & Logging
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