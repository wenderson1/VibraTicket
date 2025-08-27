# 🎯 Decisões Arquiteturais (ADRs)

<div align="center">

![ADR](https://img.shields.io/badge/ADR-Architecture_Decision_Records-purple?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Active-green?style=for-the-badge)

</div>

## 📑 Índice de Decisões

| ID | Decisão | Status | Data |
|----|---------|--------|------|
| [ADR-001](#adr-001-clean-architecture) | Adotar Clean Architecture | ✅ Aceito | 2024-01-01 |
| [ADR-002](#adr-002-cqrs-pattern) | Implementar CQRS | ✅ Aceito | 2024-01-05 |
| [ADR-003](#adr-003-entity-framework) | Usar Entity Framework Core | ✅ Aceito | 2024-01-07 |
| [ADR-004](#adr-004-validation-strategy) | FluentValidation para validações | ✅ Aceito | 2024-01-10 |
| [ADR-005](#adr-005-authentication) | JWT para autenticação | ✅ Aceito | 2024-01-12 |
| [ADR-006](#adr-006-event-sourcing) | Não usar Event Sourcing (por ora) | ✅ Aceito | 2024-01-15 |
| [ADR-007](#adr-007-api-versioning) | Versionamento via URL | ✅ Aceito | 2024-01-18 |
| [ADR-008](#adr-008-caching-strategy) | Redis para cache distribuído | 🔄 Proposto | 2024-01-20 |

---

## ADR-001: Clean Architecture

### Status
✅ Aceito - 01/01/2024

### Contexto
Precisamos de uma arquitetura que permita:
- Independência de frameworks
- Testabilidade
- Independência de UI
- Independência de banco de dados
- Separação clara de responsabilidades

### Decisão
Adotaremos Clean Architecture com as seguintes camadas:
1. **Domain**: Entidades e regras de negócio
2. **Application**: Casos de uso e lógica de aplicação
3. **Infrastructure**: Implementações técnicas
4. **API**: Interface REST

### Consequências

#### Positivas
- ✅ Alta testabilidade
- ✅ Baixo acoplamento
- ✅ Facilita mudanças tecnológicas
- ✅ Código mais organizado

#### Negativas
- ❌ Mais complexidade inicial
- ❌ Mais arquivos e abstrações
- ❌ Curva de aprendizado para novos desenvolvedores

### Exemplo
```csharp
// Domain Layer - Puro, sem dependências
public class Event : Entity
{
    public string Name { get; private set; }
    public DateTime Date { get; private set; }
    
    public void Publish()
    {
        // Lógica de negócio pura
    }
}

// Application Layer - Orquestra o domínio
public class PublishEventUseCase : IUseCase<PublishEventRequest>
{
    public async Task Execute(PublishEventRequest request)
    {
        var event = await _repository.GetById(request.EventId);
        event.Publish();
        await _repository.Update(event);
    }
}
```

---

## ADR-002: CQRS Pattern

### Status
✅ Aceito - 05/01/2024

### Contexto
- Operações de leitura são muito mais frequentes que escritas
- Modelos de leitura e escrita têm necessidades diferentes
- Queries complexas para relatórios não devem impactar o modelo de domínio

### Decisão
Implementar CQRS (Command Query Responsibility Segregation):
- **Commands**: Use Cases para operações de escrita
- **Queries**: Handlers otimizados para leitura
- Mesmo banco de dados (sem Event Sourcing)

### Consequências

#### Positivas
- ✅ Otimização independente de leituras
- ✅ Modelos específicos para cada operação
- ✅ Melhor performance em queries
- ✅ Facilita cache de leituras

#### Negativas
- ❌ Mais código para manter
- ❌ Possível duplicação de lógica
- ❌ Complexidade adicional

### Implementação
```csharp
// Command Side
public class CreateOrderCommand : ICommand<OrderResponse>
{
    public Guid CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

// Query Side  
public class GetOrdersByCustomerQuery : IQuery<List<OrderListDto>>
{
    public Guid CustomerId { get; set; }
    public OrderStatus? Status { get; set; }
}
```

---

## ADR-003: Entity Framework Core

### Status
✅ Aceito - 07/01/2024

### Contexto
Precisamos de um ORM que:
- Suporte .NET 8
- Tenha boa performance
- Permita migrations
- Suporte LINQ
- Seja maduro e bem documentado

### Decisão
Usar Entity Framework Core 8.0 com:
- Code First approach
- Fluent API para configurações
- Migrations automáticas
- Lazy loading desabilitado

### Consequências

#### Positivas
- ✅ Produtividade alta
- ✅ Type safety
- ✅ Migrations integradas
- ✅ Suporte oficial Microsoft

#### Negativas
- ❌ Performance inferior a Dapper em casos específicos
- ❌ Abstrações podem gerar queries ineficientes
- ❌ Curva de aprendizado para otimizações

### Configuração
```csharp
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        
        builder.OwnsOne(c => c.Name, name =>
        {
            name.Property(n => n.FirstName).HasMaxLength(100);
            name.Property(n => n.LastName).HasMaxLength(100);
        });
        
        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value).HasMaxLength(255);
            email.HasIndex(e => e.Value).IsUnique();
        });
    }
}
```

---

## ADR-004: Validation Strategy

### Status
✅ Aceito - 10/01/2024

### Contexto
Precisamos validar dados em múltiplas camadas:
- Entrada da API
- Regras de aplicação
- Invariantes de domínio

### Decisão
- **FluentValidation** para API e Application
- **Validação no construtor** para Domain
- **Atributos** apenas para casos simples

### Consequências

#### Positivas
- ✅ Validações testáveis
- ✅ Reutilização de regras
- ✅ Mensagens customizadas
- ✅ Validações assíncronas

#### Negativas
- ❌ Dependência adicional
- ❌ Mais classes para manter
- ❌ Possível duplicação com domínio

### Exemplo
```csharp
public class CreateEventValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventValidator(IEventRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
            
        RuleFor(x => x.VenueId)
            .MustAsync(async (id, ct) => await repository.VenueExists(id))
            .WithMessage("Venue not found");
            
        RuleFor(x => x.Date)
            .GreaterThan(DateTime.Now)
            .WithMessage("Event must be in the future");
    }
}
```

---

## ADR-005: Authentication Strategy

### Status
✅ Aceito - 12/01/2024

### Contexto
- API pública precisa de autenticação
- Diferentes níveis de permissão (Customer, Admin, Affiliate)
- Precisa ser stateless para escalabilidade
- Suporte para refresh tokens

### Decisão
Implementar autenticação via JWT com:
- Access Token (15 minutos)
- Refresh Token (7 dias)
- Claims para roles e permissões
- Middleware customizado

### Consequências

#### Positivas
- ✅ Stateless e escalável
- ✅ Padrão da indústria
- ✅ Suporte em múltiplas plataformas
- ✅ Fácil integração com frontend

#### Negativas
- ❌ Não pode revogar tokens facilmente
- ❌ Tamanho maior que session cookies
- ❌ Precisa gerenciar refresh tokens

### Implementação
```csharp
public class JwtService : IJwtService
{
    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: _credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

---

## ADR-006: Event Sourcing Decision

### Status
✅ Aceito - 15/01/2024

### Contexto
- Sistema de eventos naturalmente se beneficiaria de Event Sourcing
- Necessidade de auditoria completa
- Possibilidade de replay de eventos

### Decisão
**NÃO** implementar Event Sourcing nesta fase:
- Usar apenas Domain Events para comunicação
- Manter estado atual em tabelas tradicionais
- Logs de auditoria em tabela separada

### Razões
1. Complexidade adicional significativa
2. Time sem experiência com Event Sourcing
3. MVP precisa ser entregue rapidamente
4. Pode ser migrado no futuro se necessário

### Consequências

#### Positivas
- ✅ Simplicidade de implementação
- ✅ Ferramentas conhecidas
- ✅ Menor curva de aprendizado
- ✅ Debugging mais simples

#### Negativas
- ❌ Sem histórico completo de mudanças
- ❌ Auditoria mais limitada
- ❌ Migração futura mais complexa

---

## ADR-007: API Versioning

### Status
✅ Aceito - 18/01/2024

### Contexto
- API será consumida por múltiplos clientes
- Mudanças breaking changes são inevitáveis
- Precisa suportar múltiplas versões simultaneamente

### Decisão
Versionamento via URL:
```
https://api.vibraticket.com/v1/events
https://api.vibraticket.com/v2/events
```

### Alternativas Consideradas
1. **Header Versioning**: `API-Version: 1.0`
2. **Query String**: `/events?version=1`
3. **Media Type**: `Accept: application/vnd.vibraticket.v1+json`

### Consequências

#### Positivas
- ✅ Simples e claro
- ✅ Fácil de cachear
- ✅ Funciona bem com Swagger
- ✅ URLs autodocumentadas

#### Negativas
- ❌ URLs menos "RESTful"
- ❌ Duplicação de rotas
- ❌ Mudanças em URLs

---

## ADR-008: Caching Strategy

### Status
🔄 Proposto - 20/01/2024

### Contexto
- Muitas consultas repetidas (eventos, setores)
- Necessidade de reduzir carga no banco
- Dados de eventos mudam pouco após publicação
- Sistema distribuído no futuro

### Decisão Proposta
Implementar cache em múltiplas camadas:
1. **Memory Cache**: Dados hot (configurações)
2. **Redis**: Cache distribuído para queries
3. **Response Cache**: HTTP caching

### Estratégia
```csharp
public class CachedEventQuery : IEventQuery
{
    private readonly IEventQuery _innerQuery;
    private readonly IDistributedCache _cache;
    
    public async Task<EventDto> GetById(Guid id)
    {
        var key = $"event:{id}";
        var cached = await _cache.GetAsync<EventDto>(key);
        
        if (cached != null) 
            return cached;
            
        var result = await _innerQuery.GetById(id);
        await _cache.SetAsync(key, result, TimeSpan.FromMinutes(15));
        
        return result;
    }
}
```

### Pontos de Atenção
- Invalidação de cache
- Consistência eventual
- Memória vs Performance
- Complexidade operacional

---

## 📋 Template para Novas ADRs

```markdown
## ADR-XXX: [Título da Decisão]

### Status
🔄 Proposto / ✅ Aceito / ❌ Rejeitado / 🔄 Substituído - DD/MM/YYYY

### Contexto
[Descreva o contexto e o problema que precisa ser resolvido]

### Decisão
[Descreva a decisão tomada]

### Alternativas Consideradas
1. [Alternativa 1]
2. [Alternativa 2]

### Consequências

#### Positivas
- ✅ [Consequência positiva]

#### Negativas
- ❌ [Consequência negativa]

### Implementação
[Exemplo de código ou diagrama se aplicável]
```

---

<div align="center">

[← Endpoints](./endpoints.md) | [Próximo: Padrões →](./patterns.md)

</div>