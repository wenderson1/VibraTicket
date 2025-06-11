# VibraTicket

Sistema de gerenciamento de venda de ingressos para eventos, desenvolvido em .NET 8 seguindo princ�pios de Clean Architecture e DDD.

## ??? Arquitetura

O projeto � estruturado em camadas seguindo os princ�pios de Clean Architecture:
src/
??? Domain/           # Entidades e regras de neg�cio core
??? Application/      # Casos de uso e l�gica de aplica��o
??? Infrastructure/   # Implementa��es de persist�ncia e servi�os externos
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

- Gest�o de Eventos
- Gest�o de Setores
- Gest�o de Ingressos
- Gest�o de Clientes
- Gest�o de Afiliados
- Gest�o de Vendas
- Sistema de Pagamentos

## ?? Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **SQL Server**: Banco de dados
- **FluentValidation**: Valida��o de dados
- **Swagger**: Documenta��o da API
- **CQRS Pattern**: Separa��o de comandos e consultas
- **Clean Architecture**: Arquitetura limpa e desacoplada

## ?? Padr�es de Projeto

- **CQRS**: Separa��o de comandos e consultas
- **Repository Pattern**: Abstra��o do acesso a dados
- **Unit of Work**: Controle de transa��es
- **Dependency Injection**: Invers�o de controle
- **Factory Pattern**: Cria��o de objetos
- **Validator Pattern**: Valida��o de dados

## ?? Design da Solu��o

### Domain Layer
- Entidades de dom�nio
- Regras de neg�cio
- Interfaces dos reposit�rios

### Application Layer
- Use Cases (Commands)
- Queries
- Validadores
- DTOs
- Interfaces

### Infrastructure Layer
- Implementa��es dos reposit�rios
- Contexto do Entity Framework
- Configura��es de entidades
- Unit of Work

### API Layer
- Controllers
- Middleware
- Configura��es
- Dependency Injection

## ?? Como Executar

1. Clone o reposit�rio
2. Configure a string de conex�o no `appsettings.json`
3. Execute as migra��es:dotnet ef database update4. Execute o projeto:dotnet run --project src/Api/Api.csproj
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
## ?? Detalhes T�cnicos

### Entidades Principais
- Customer
- Affiliate
- Event
- Sector
- Ticket
- Order
- Payment
- Venue

### Padr�es de API
- REST
- Documenta��o Swagger
- Versionamento de API
- Tratamento padronizado de erros
- Logs estruturados

## ?? Boas Pr�ticas

- Logging extensivo
- Valida��o de dados
- Tratamento de erros
- Clean Code
- SOLID Principles
- DDD Concepts
- Testes unit�rios
- Documenta��o clara

## ?? Seguran�a

- Valida��o de dados de entrada
- Sanitiza��o de dados
- Prote��o contra inje��o SQL
- Logs de auditoria
- Filtros de seguran�a

## ?? Contribuindo

1. Fa�a um fork do projeto
2. Crie uma branch para sua feature
3. Commit suas mudan�as
4. Push para a branch
5. Abra um Pull Request

## ?? Licen�a

Este projeto est� sob a licen�a [MIT](LICENSE).
