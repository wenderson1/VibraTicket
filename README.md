# VibraTicket API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

## 📋 Sobre

**VibraTicket** é uma API RESTful completa para gerenciamento de eventos e venda de ingressos, construída com .NET 8 e seguindo os princípios de Clean Architecture, DDD e CQRS. A solução oferece um sistema robusto e escalável para gestão de eventos, setores, ingressos, pedidos e pagamentos.

## 🏗️ Arquitetura

O projeto segue Clean Architecture com as seguintes camadas:

```
src/
├── Domain/          # Entidades, Value Objects, Regras de Negócio
├── Application/     # Casos de Uso, DTOs, Interfaces, Queries
├── Infrastructure/  # Implementação de Repositórios, EF Core, Serviços Externos
└── Api/            # Controllers, Configurações, Middleware
```

### Princípios Adotados

- **Clean Architecture**: Separação clara de responsabilidades
- **Domain-Driven Design (DDD)**: Foco no domínio do negócio
- **CQRS**: Separação entre comandos (UseCases) e consultas (Queries)
- **Repository Pattern**: Abstração do acesso a dados
- **Unit of Work**: Gerenciamento de transações
- **Dependency Injection**: Baixo acoplamento entre componentes

## 🚀 Tecnologias

- **.NET 8.0** - Framework principal
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Banco de dados
- **FluentValidation** - Validação de dados
- **Swagger/OpenAPI** - Documentação da API
- **Serilog** - Logging estruturado (opcional)

## 📦 Funcionalidades

### Módulos Principais

- **👥 Clientes (Customers)**
  - Cadastro, atualização e exclusão
  - Busca por ID, documento ou email
  - Validação de documentos (CPF/CNPJ)

- **🏢 Afiliados (Affiliates)**
  - Gestão completa de afiliados
  - Controle de comissões e vendas

- **📍 Locais (Venues)**
  - Cadastro de locais de eventos
  - Informações de capacidade e endereço

- **🎭 Eventos (Events)**
  - Criação e gerenciamento de eventos
  - Configuração de datas e horários
  - Associação com locais

- **🎫 Setores (Sectors)**
  - Definição de setores do evento
  - Configuração de preços e capacidade

- **🎟️ Ingressos (Tickets)**
  - Geração e controle de ingressos
  - Status e validação

- **🛒 Pedidos (Orders)**
  - Carrinho de compras
  - Processamento de pedidos
  - Histórico de transações

- **💳 Pagamentos (Payments)**
  - Integração com gateways de pagamento
  - Controle de status de pagamento

## ⚙️ Instalação e Configuração

### Pré-requisitos

- .NET 8.0 SDK ou superior
- SQL Server 2019 ou superior
- Visual Studio 2022 ou VS Code

### Passo a Passo

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/vibraticket.git
   cd vibraticket
   ```

2. **Configure o banco de dados**
   
   Edite o arquivo `appsettings.json` com sua connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=VibraTicket;User Id=sa;Password=SuaSenha123;TrustServerCertificate=True"
     }
   }
   ```

3. **Execute as migrações**
   ```bash
   cd src/Infrastructure
   dotnet ef database update
   ```

4. **Execute a aplicação**
   ```bash
   cd ../Api
   dotnet run
   ```

5. **Acesse a documentação**
   
   Abra seu navegador em: `https://localhost:7001/swagger`

## 🔧 Configurações Adicionais

### Ambientes

O projeto suporta múltiplos ambientes:
- `Development`: Para desenvolvimento local
- `Staging`: Para testes
- `Production`: Para produção

Configure usando:
```bash
dotnet run --environment Development
```

### CORS

A API está configurada para permitir requisições de qualquer origem em desenvolvimento. Para produção, ajuste em `Program.cs`:

```csharp
options.AddPolicy("Production",
    policy => policy
        .WithOrigins("https://seu-dominio.com")
        .AllowAnyMethod()
        .AllowAnyHeader());
```

### Autenticação e Autorização

Por padrão, a API está sem autenticação. Para habilitar JWT:

1. Descomente as linhas em `Program.cs`:
   ```csharp
   app.UseAuthentication(); 
   app.UseAuthorization();
   ```

2. Configure JWT em `appsettings.json`

## 📝 Exemplos de Uso

### Criar um Cliente

```http
POST /api/customer
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao@email.com",
  "document": "12345678901",
  "phone": "11999999999"
}
```

### Buscar Eventos

```http
GET /api/event
```

### Criar um Pedido

```http
POST /api/order
Content-Type: application/json

{
  "customerId": "guid-do-cliente",
  "items": [
    {
      "sectorId": "guid-do-setor",
      "quantity": 2
    }
  ]
}
```

## 🧪 Testes

Execute os testes com:
```bash
dotnet test
```

## 📊 Monitoramento

### Health Check

```http
GET /health
```

### Métricas

Integração com Application Insights ou Prometheus pode ser configurada conforme necessidade.

## 🤝 Contribuindo

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### Padrões de Código

- Siga as convenções C# da Microsoft
- Mantenha cobertura de testes acima de 80%
- Documente métodos públicos
- Use mensagens de commit significativas

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👥 Autores

- **Seu Nome** - *Desenvolvimento inicial* - [seu-github](https://github.com/seu-usuario)

## 🙏 Agradecimentos

- Time de desenvolvimento
- Comunidade .NET
- Contribuidores do projeto

---

**VibraTicket API** - Transformando a experiência de eventos 🎉