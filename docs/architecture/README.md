# 🏗️ Arquitetura do VibraTicket

<div align="center">

![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-brightgreen?style=for-the-badge)
![Pattern](https://img.shields.io/badge/Pattern-CQRS-blue?style=for-the-badge)
![Design](https://img.shields.io/badge/Design-DDD-orange?style=for-the-badge)

</div>

## 📑 Índice de Documentação

| Documento | Descrição |
|-----------|-----------|
| 📐 [Camadas da Arquitetura](./layers.md) | Detalhamento de cada camada e suas responsabilidades |
| 🔄 [Fluxo de Dados](./data-flow.md) | Como os dados fluem através do sistema |
| 📊 [Modelo de Domínio](./domain-model.md) | Entidades e seus relacionamentos |
| 🔌 [Endpoints da API](./endpoints.md) | Mapeamento completo dos endpoints REST |
| 🎯 [Decisões Arquiteturais](./decisions.md) | ADRs e escolhas técnicas |
| 🔧 [Padrões e Práticas](./patterns.md) | Padrões de design implementados |

## 🎯 Visão Geral

O VibraTicket foi projetado seguindo os princípios de **Clean Architecture** (Arquitetura Limpa), garantindo:

- ✅ **Independência de Frameworks**: O núcleo da aplicação não depende de bibliotecas externas
- ✅ **Testabilidade**: Facilita a criação de testes em todos os níveis
- ✅ **Independência de UI**: A lógica de negócio não sabe como será apresentada
- ✅ **Independência de Banco de Dados**: Pode trocar de SQL Server para outro sem afetar o domínio
- ✅ **Independência de Agentes Externos**: Mudanças em APIs externas não afetam o núcleo

## 🏛️ Arquitetura em Camadas

```mermaid
graph TB
    subgraph "🌐 Presentation Layer"
        API[REST API<br/>Controllers & Endpoints]
        DOCS[Swagger/OpenAPI<br/>Documentation]
    end

    subgraph "💼 Application Layer"
        UC[Use Cases<br/>Business Operations]
        Q[Queries<br/>Read Operations]
        DTO[DTOs<br/>Data Transfer]
        VAL[Validators<br/>Input Validation]
        MAP[Mappers<br/>Object Mapping]
    end

    subgraph "🎯 Domain Layer"
        ENT[Entities<br/>Business Objects]
        VO[Value Objects<br/>Domain Concepts]
        AGG[Aggregates<br/>Consistency Boundaries]
        RULE[Domain Rules<br/>Business Logic]
        EVT[Domain Events<br/>State Changes]
        DINT[Domain Interfaces<br/>Contracts]
    end

    subgraph "🔧 Infrastructure Layer"
        REPO[Repositories<br/>Data Access]
        UOW[Unit of Work<br/>Transactions]
        EF[Entity Framework<br/>ORM]
        DB[(SQL Server<br/>Database)]
        EMAIL[Email Service<br/>Notifications]
        PAY[Payment Gateway<br/>External API]
    end

    API --> UC
    API --> Q
    DOCS --> API
    UC --> DTO
    Q --> DTO
    UC --> VAL
    UC --> MAP
    UC --> RULE
    UC --> ENT
    Q --> DINT
    UC --> DINT
    RULE --> ENT
    ENT --> VO
    ENT --> AGG
    RULE --> EVT
    REPO --> EF
    UOW --> EF
    EF --> DB
    REPO -.-> DINT
    EMAIL -.-> DINT
    PAY -.-> DINT

    classDef presentation fill:#e3f2fd,stroke:#1565c0,stroke-width:3px
    classDef application fill:#f3e5f5,stroke:#6a1b9a,stroke-width:3px
    classDef domain fill:#fff8e1,stroke:#f57f17,stroke-width:3px
    classDef infrastructure fill:#fce4ec,stroke:#ad1457,stroke-width:3px

    class API,DOCS presentation
    class UC,Q,DTO,VAL,MAP application
    class ENT,VO,AGG,RULE,EVT,DINT domain
    class REPO,UOW,EF,DB,EMAIL,PAY infrastructure
```

## 🔄 Fluxo de Dependências

```mermaid
graph LR
    subgraph "Direção das Dependências"
        P[Presentation]
        A[Application]
        D[Domain]
        I[Infrastructure]
        
        P --> A
        A --> D
        I --> D
        I --> A
    end

    subgraph "Regra de Dependência"
        direction TB
        RULE[📋 As dependências devem<br/>apontar apenas para dentro]
        CORE[🎯 O Domain é o núcleo<br/>e não depende de nada]
    end
```

## 📊 Stack Tecnológico

```mermaid
mindmap
  root((VibraTicket))
    Backend
      .NET 8
        C# 12
        Minimal APIs
        Native AOT Ready
      Entity Framework Core 8
        Code First
        Migrations
        Query Optimization
      SQL Server
        Stored Procedures
        Indexes
        Views
    Patterns
      Clean Architecture
        Use Cases
        Interfaces
        Dependency Inversion
      CQRS
        Commands
        Queries
        Handlers
      Domain-Driven Design
        Entities
        Value Objects
        Aggregates
    Libraries
      FluentValidation
        Request Validation
        Business Rules
        Custom Validators
      Swagger/OpenAPI
        API Documentation
        Try it out
        Schema Generation
      Serilog
        Structured Logging
        Sinks
        Enrichers
```

## 🎨 Padrões e Princípios

### SOLID Principles

| Princípio | Aplicação no Projeto |
|-----------|---------------------|
| **S**ingle Responsibility | Cada classe tem apenas uma razão para mudar |
| **O**pen/Closed | Extensível sem modificar código existente |
| **L**iskov Substitution | Interfaces bem definidas e respeitadas |
| **I**nterface Segregation | Interfaces pequenas e específicas |
| **D**ependency Inversion | Dependências sempre apontam para abstrações |

### Design Patterns Implementados

```mermaid
graph TD
    subgraph "Padrões Criacionais"
        FACTORY[Factory Pattern<br/>Para criar entidades]
        BUILDER[Builder Pattern<br/>Para objetos complexos]
    end

    subgraph "Padrões Estruturais"
        REPO[Repository Pattern<br/>Abstração de dados]
        ADAPTER[Adapter Pattern<br/>Integração externa]
        FACADE[Facade Pattern<br/>API simplificada]
    end

    subgraph "Padrões Comportamentais"
        STRATEGY[Strategy Pattern<br/>Algoritmos intercambiáveis]
        OBSERVER[Observer Pattern<br/>Domain Events]
        SPEC[Specification Pattern<br/>Regras de negócio]
    end

    classDef creation fill:#c8e6c9,stroke:#388e3c
    classDef structural fill:#b3e5fc,stroke:#0277bd
    classDef behavioral fill:#ffe0b2,stroke:#ef6c00

    class FACTORY,BUILDER creation
    class REPO,ADAPTER,FACADE structural
    class STRATEGY,OBSERVER,SPEC behavioral
```

## 🔒 Segurança em Camadas

```mermaid
graph TB
    subgraph "Security Layers"
        AUTH[Authentication<br/>JWT Tokens]
        AUTHZ[Authorization<high/>Role-Based]
        VAL[Validation<br/>Input Sanitization]
        AUDIT[Audit<br/>Action Logging]
        CRYPTO[Cryptography<br/>Data Protection]
    end

    REQ[Request] --> AUTH
    AUTH --> AUTHZ
    AUTHZ --> VAL
    VAL --> AUDIT
    AUDIT --> CRYPTO
    CRYPTO --> APP[Application]

    classDef security fill:#ffebee,stroke:#c62828
    class AUTH,AUTHZ,VAL,AUDIT,CRYPTO security
```

## 📈 Escalabilidade

### Estratégias de Escalabilidade

- **Horizontal Scaling**: Aplicação stateless permite múltiplas instâncias
- **Database Scaling**: Read replicas e particionamento
- **Caching**: Redis para cache distribuído
- **Async Processing**: Filas para operações pesadas
- **CDN**: Para assets estáticos

### Preparação para Microserviços

O design modular permite fácil transição para microserviços:

```mermaid
graph LR
    subgraph "Monolith Atual"
        MONO[VibraTicket<br/>Monolith]
    end

    subgraph "Futura Arquitetura de Microserviços"
        MS1[Customer<br/>Service]
        MS2[Event<br/>Service]
        MS3[Order<br/>Service]
        MS4[Payment<br/>Service]
        MS5[Notification<br/>Service]
        
        GW[API Gateway]
        
        GW --> MS1
        GW --> MS2
        GW --> MS3
        GW --> MS4
        GW --> MS5
    end

    MONO -.-> GW

    classDef current fill:#e8f5e9,stroke:#2e7d32
    classDef future fill:#e3f2fd,stroke:#1565c0

    class MONO current
    class MS1,MS2,MS3,MS4,MS5,GW future
```

## 📚 Referências e Leitura Adicional

### Livros
- **"Clean Architecture"** - Robert C. Martin
- **"Domain-Driven Design"** - Eric Evans
- **"Implementing Domain-Driven Design"** - Vaughn Vernon

### Artigos
- [Clean Architecture with .NET](https://docs.microsoft.com/clean-architecture)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [Repository Pattern Done Right](https://www.programmingwithwolfgang.com/repository-pattern-done-right/)

### Recursos Online
- [.NET Architecture Guides](https://dotnet.microsoft.com/learn/dotnet/architecture-guides)
- [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture)

---

<div align="center">

[← Voltar](../README.md) | [Próximo: Camadas →](./layers.md)

</div>