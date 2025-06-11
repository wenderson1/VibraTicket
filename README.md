# VibraTicket

Sistema de gerenciamento de eventos e venda de ingressos, desenvolvido em .NET 8 com Clean Architecture.

## ? Visão Geral

O VibraTicket é uma solução modular para gestão de eventos, clientes, afiliados, setores, ingressos, pedidos e pagamentos. O projeto segue os princípios de Clean Architecture, DDD e CQRS, promovendo desacoplamento, testabilidade e escalabilidade.

## ??? Arquitetura

- **Domain:** Entidades, regras de negócio e contratos.
- **Application:** Casos de uso (UseCases), Queries, Validadores e DTOs.
- **Infrastructure:** Persistência (EF Core), repositórios, Unit of Work.
- **API:** Controllers REST, configuração de DI, documentação Swagger.

Veja o diagrama detalhado em [`docs/architecture.md`](docs/architecture.md).

## ?? Como Executar

1. Clone o repositório
2. Configure a string de conexão no `appsettings.json`
3. Execute as migrações:dotnet ef database update4. Rode a aplicação:dotnet run --project src/Api/Api.csproj
## ?? Estrutura de Pastas
src/
??? Domain/
??? Application/
??? Infrastructure/
??? Api/
## ?? Tecnologias

- .NET 8
- Entity Framework Core
- SQL Server
- FluentValidation
- Swagger/OpenAPI

## ?? Padrões e Práticas

- Clean Architecture
- CQRS (UseCases/Queries)
- Repository & Unit of Work
- Dependency Injection
- Validação com FluentValidation
- Logging estruturado
- Tratamento padronizado de erros

## ?? Funcionalidades

- CRUD de Clientes, Afiliados, Eventos, Setores, Ingressos, Pedidos e Pagamentos
- Consultas por ID, documento e e-mail
- Validação e tratamento de erros centralizados
- API RESTful documentada

## ?? Contribuição

1. Fork este repositório
2. Crie uma branch para sua feature
3. Commit e push
4. Abra um Pull Request

## ?? Licença

MIT
