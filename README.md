# VibraTicket

Sistema de gerenciamento de eventos e venda de ingressos, desenvolvido em .NET 8 com Clean Architecture.

## ? Vis�o Geral

O VibraTicket � uma solu��o modular para gest�o de eventos, clientes, afiliados, setores, ingressos, pedidos e pagamentos. O projeto segue os princ�pios de Clean Architecture, DDD e CQRS, promovendo desacoplamento, testabilidade e escalabilidade.

## ??? Arquitetura

- **Domain:** Entidades, regras de neg�cio e contratos.
- **Application:** Casos de uso (UseCases), Queries, Validadores e DTOs.
- **Infrastructure:** Persist�ncia (EF Core), reposit�rios, Unit of Work.
- **API:** Controllers REST, configura��o de DI, documenta��o Swagger.

Veja o diagrama detalhado em [`docs/architecture.md`](docs/architecture.md).

## ?? Como Executar

1. Clone o reposit�rio
2. Configure a string de conex�o no `appsettings.json`
3. Execute as migra��es:dotnet ef database update4. Rode a aplica��o:dotnet run --project src/Api/Api.csproj
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

## ?? Padr�es e Pr�ticas

- Clean Architecture
- CQRS (UseCases/Queries)
- Repository & Unit of Work
- Dependency Injection
- Valida��o com FluentValidation
- Logging estruturado
- Tratamento padronizado de erros

## ?? Funcionalidades

- CRUD de Clientes, Afiliados, Eventos, Setores, Ingressos, Pedidos e Pagamentos
- Consultas por ID, documento e e-mail
- Valida��o e tratamento de erros centralizados
- API RESTful documentada

## ?? Contribui��o

1. Fork este reposit�rio
2. Crie uma branch para sua feature
3. Commit e push
4. Abra um Pull Request

## ?? Licen�a

MIT
