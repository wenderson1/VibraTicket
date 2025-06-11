# VibraTicket

Sistema de gerenciamento de venda de ingressos para eventos, desenvolvido em .NET 8 seguindo princípios de Clean Architecture e DDD.

## ??? Arquitetura

O projeto é estruturado em camadas seguindo os princípios de Clean Architecture:
src/
??? Domain/           # Entidades e regras de negócio core
??? Application/      # Casos de uso e lógica de aplicação
??? Infrastructure/   # Implementações de persistência e serviços externos
??? Api/             # Interface da API REST
### Diagrama da Arquitetura
graph TB
    API[API Layer] --> APP[Application Layer]
    APP --> DOMAIN[Domain Layer]
    INFRA[Infrastructure Layer] --> APP
    INFRA --> DOMAIN
    
    subgraph API
        CONT[Controllers]
        MWARE[Middleware]
        DI[Dependency Injection]
    end
    
    subgraph Application
        UC[Use Cases]
        PORT[Ports/Interfaces]
        QUERY[Queries]
        VAL[Validators]
    end
    
    subgraph Domain
        ENT[Entities]
        VO[Value Objects]
        REPO[Repository Interfaces]
    end
    
    subgraph Infrastructure
        DB[Database]
        REPOS[Repositories]
        UOW[Unit of Work]
    end
## ?? Funcionalidades Principais

- Gestão de Eventos
- Gestão de Setores
- Gestão de Ingressos
- Gestão de Clientes
- Gestão de Afiliados
- Gestão de Vendas
- Sistema de Pagamentos

## ?? Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **SQL Server**: Banco de dados
- **FluentValidation**: Validação de dados
- **Swagger**: Documentação da API
- **CQRS Pattern**: Separação de comandos e consultas
- **Clean Architecture**: Arquitetura limpa e desacoplada

## ?? Padrões de Projeto

- **CQRS**: Separação de comandos e consultas
- **Repository Pattern**: Abstração do acesso a dados
- **Unit of Work**: Controle de transações
- **Dependency Injection**: Inversão de controle
- **Factory Pattern**: Criação de objetos
- **Validator Pattern**: Validação de dados

## ?? Design da Solução

### Domain Layer
- Entidades de domínio
- Regras de negócio
- Interfaces dos repositórios

### Application Layer
- Use Cases (Commands)
- Queries
- Validadores
- DTOs
- Interfaces

### Infrastructure Layer
- Implementações dos repositórios
- Contexto do Entity Framework
- Configurações de entidades
- Unit of Work

### API Layer
- Controllers
- Middleware
- Configurações
- Dependency Injection

## ?? Como Executar

1. Clone o repositório
2. Configure a string de conexão no `appsettings.json`
3. Execute as migrações:dotnet ef database update4. Execute o projeto:dotnet run --project src/Api/Api.csproj
## ?? Estrutura de Pastas
src/
??? Domain/
?   ??? Entities/
??? Application/
?   ??? UseCases/
?   ??? Query/
?   ??? Commons/
?   ??? Interfaces/
??? Infrastructure/
?   ??? Persistence/
?   ??? Repositories/
??? Api/
    ??? Controllers/
    ??? Configurations/
    ??? Common/
## ?? Detalhes Técnicos

### Entidades Principais
- Customer
- Affiliate
- Event
- Sector
- Ticket
- Order
- Payment
- Venue

### Padrões de API
- REST
- Documentação Swagger
- Versionamento de API
- Tratamento padronizado de erros
- Logs estruturados

## ?? Boas Práticas

- Logging extensivo
- Validação de dados
- Tratamento de erros
- Clean Code
- SOLID Principles
- DDD Concepts
- Testes unitários
- Documentação clara

## ?? Segurança

- Validação de dados de entrada
- Sanitização de dados
- Proteção contra injeção SQL
- Logs de auditoria
- Filtros de segurança

## ?? Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## ?? Licença

Este projeto está sob a licença [MIT](LICENSE).
