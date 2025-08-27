# 🔌 Endpoints da API

<div align="center">

![REST](https://img.shields.io/badge/API-RESTful-green?style=for-the-badge)
![OpenAPI](https://img.shields.io/badge/OpenAPI-3.0-blue?style=for-the-badge)
![Version](https://img.shields.io/badge/Version-1.0-orange?style=for-the-badge)

</div>

## 📑 Índice

- [🎯 Visão Geral](#-visão-geral)
- [🔐 Autenticação](#-autenticação)
- [📋 Recursos Disponíveis](#-recursos-disponíveis)
- [🎫 Customer Endpoints](#-customer-endpoints)
- [🎭 Event Endpoints](#-event-endpoints)
- [🛒 Order Endpoints](#-order-endpoints)
- [💳 Payment Endpoints](#-payment-endpoints)
- [📊 Reports Endpoints](#-reports-endpoints)
- [🔍 Códigos de Status](#-códigos-de-status)
- [⚠️ Tratamento de Erros](#️-tratamento-de-erros)

## 🎯 Visão Geral

A API VibraTicket segue os princípios REST e está documentada usando OpenAPI 3.0. Todos os endpoints retornam JSON e usam códigos de status HTTP padrão.

### Base URL
```
Production: https://api.vibraticket.com/v1
Staging:    https://staging-api.vibraticket.com/v1
Local:      https://localhost:7001/api/v1
```

### Headers Padrão
```http
Content-Type: application/json
Accept: application/json
Authorization: Bearer {token}
X-Request-ID: {uuid}
```

## 🔐 Autenticação

### Login
```http
POST /auth/login
Content-Type: application/json

{
    "email": "user@example.com",
    "password": "securepassword"
}

Response 200 OK:
{
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "fdb8fdbecf1d0397...",
    "expiresIn": 3600,
    "tokenType": "Bearer"
}
```

### Refresh Token
```http
POST /auth/refresh
Content-Type: application/json

{
    "refreshToken": "fdb8fdbecf1d0397..."
}

Response 200 OK:
{
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "expiresIn": 3600
}
```

## 📋 Recursos Disponíveis

```mermaid
graph LR
    subgraph "Public Endpoints"
        PE1[GET /events]
        PE2[GET /events/{id}]
        PE3[GET /venues]
        PE4[POST /auth/login]
        PE5[POST /customers]
    end

    subgraph "Authenticated Endpoints"
        AE1[GET /customers/profile]
        AE2[POST /orders]
        AE3[GET /orders/history]
        AE4[GET /tickets]
    end

    subgraph "Admin Endpoints"
        AD1[POST /events]
        AD2[PUT /events/{id}]
        AD3[GET /reports]
        AD4[GET /affiliates]
    end

    classDef public fill:#e8f5e9,stroke:#388e3c
    classDef auth fill:#fff3e0,stroke:#f57c00
    classDef admin fill:#ffebee,stroke:#c62828

    class PE1,PE2,PE3,PE4,PE5 public
    class AE1,AE2,AE3,AE4 auth
    class AD1,AD2,AD3,AD4 admin
```

## 🎫 Customer Endpoints

### Create Customer
```http
POST /customers
Content-Type: application/json

{
    "firstName": "João",
    "lastName": "Silva",
    "email": "joao.silva@email.com",
    "document": "12345678901",
    "phone": "+5511999999999",
    "type": "INDIVIDUAL"
}

Response 201 Created:
{
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "firstName": "João",
    "lastName": "Silva",
    "email": "joao.silva@email.com",
    "document": "123.456.789-01",
    "phone": "+55 11 99999-9999",
    "type": "INDIVIDUAL",
    "status": "ACTIVE",
    "createdAt": "2024-01-15T10:30:00Z"
}
```

### Get Customer by ID
```http
GET /customers/{id}
Authorization: Bearer {token}

Response 200 OK:
{
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "firstName": "João",
    "lastName": "Silva",
    "email": "joao.silva@email.com",
    "document": "123.456.789-01",
    "phone": "+55 11 99999-9999",
    "type": "INDIVIDUAL",
    "status": "ACTIVE",
    "createdAt": "2024-01-15T10:30:00Z",
    "stats": {
        "totalOrders": 5,
        "totalSpent": 1250.00,
        "eventsAttended": 3
    }
}
```

### Update Customer
```http
PUT /customers/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
    "firstName": "João",
    "lastName": "Silva Santos",
    "phone": "+5511888888888"
}

Response 200 OK:
{
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "firstName": "João",
    "lastName": "Silva Santos",
    "email": "joao.silva@email.com",
    "document": "123.456.789-01",
    "phone": "+55 11 88888-8888",
    "type": "INDIVIDUAL",
    "status": "ACTIVE",
    "updatedAt": "2024-01-15T11:00:00Z"
}
```

### Search Customers
```http
GET /customers?email=joao@email.com
GET /customers?document=12345678901
GET /customers?page=1&size=20&sort=createdAt,desc
Authorization: Bearer {token}

Response 200 OK:
{
    "content": [
        {
            "id": "550e8400-e29b-41d4-a716-446655440000",
            "firstName": "João",
            "lastName": "Silva",
            "email": "joao.silva@email.com",
            "type": "INDIVIDUAL",
            "status": "ACTIVE"
        }
    ],
    "page": {
        "size": 20,
        "number": 1,
        "totalElements": 1,
        "totalPages": 1
    }
}
```

## 🎭 Event Endpoints

### List Events
```http
GET /events?status=PUBLISHED&venue=123&date=2024-01-20
GET /events?search=rock&minPrice=50&maxPrice=200

Response 200 OK:
{
    "content": [
        {
            "id": "650e8400-e29b-41d4-a716-446655440001",
            "name": "Rock Festival 2024",
            "description": "O maior festival de rock do Brasil",
            "startDate": "2024-06-15T18:00:00Z",
            "endDate": "2024-06-16T04:00:00Z",
            "venue": {
                "id": "750e8400-e29b-41d4-a716-446655440002",
                "name": "Estádio Nacional",
                "address": "Brasília, DF"
            },
            "priceRange": {
                "min": 80.00,
                "max": 350.00
            },
            "availableTickets": 15000,
            "totalCapacity": 50000,
            "imageUrl": "https://cdn.vibraticket.com/events/rock-festival-2024.jpg"
        }
    ],
    "page": {
        "size": 20,
        "number": 1,
        "totalElements": 45,
        "totalPages": 3
    }
}
```

### Get Event Details
```http
GET /events/{id}

Response 200 OK:
{
    "id": "650e8400-e29b-41d4-a716-446655440001",
    "name": "Rock Festival 2024",
    "description": "O maior festival de rock do Brasil",
    "startDate": "2024-06-15T18:00:00Z",
    "endDate": "2024-06-16T04:00:00Z",
    "status": "PUBLISHED",
    "venue": {
        "id": "750e8400-e29b-41d4-a716-446655440002",
        "name": "Estádio Nacional",
        "address": "Brasília, DF",
        "capacity": 50000,
        "amenities": ["Parking", "Food Court", "VIP Area"]
    },
    "affiliate": {
        "id": "850e8400-e29b-41d4-a716-446655440003",
        "name": "Rock Productions",
        "rating": 4.8
    },
    "sectors": [
        {
            "id": "950e8400-e29b-41d4-a716-446655440004",
            "name": "Pista Premium",
            "price": 180.00,
            "capacity": 20000,
            "available": 5420,
            "description": "Acesso próximo ao palco"
        },
        {
            "id": "950e8400-e29b-41d4-a716-446655440005",
            "name": "Pista Comum",
            "price": 80.00,
            "capacity": 25000,
            "available": 8700,
            "description": "Acesso geral"
        },
        {
            "id": "950e8400-e29b-41d4-a716-446655440006",
            "name": "Camarote VIP",
            "price": 350.00,
            "capacity": 5000,
            "available": 880,
            "description": "Area VIP com serviço exclusivo"
        }
    ],
    "rules": [
        "Entrada a partir de 18 anos",
        "Proibido entrar com bebidas",
        "Documento com foto obrigatório"
    ],
    "imageUrls": {
        "main": "https://cdn.vibraticket.com/events/rock-festival-2024.jpg",
        "gallery": [
            "https://cdn.vibraticket.com/events/rock-festival-2024-1.jpg",
            "https://cdn.vibraticket.com/events/rock-festival-2024-2.jpg"
        ]
    }
}
```

### Create Event (Admin)
```http
POST /events
Authorization: Bearer {admin_token}
Content-Type: application/json

{
    "name": "Jazz Night 2024",
    "description": "Uma noite inesquecível de jazz",
    "startDate": "2024-07-20T20:00:00Z",
    "endDate": "2024-07-21T02:00:00Z",
    "venueId": "750e8400-e29b-41d4-a716-446655440002",
    "affiliateId": "850e8400-e29b-41d4-a716-446655440003",
    "sectors": [
        {
            "name": "Mesa VIP",
            "price": 250.00,
            "capacity": 100
        },
        {
            "name": "Plateia",
            "price": 120.00,
            "capacity": 300
        }
    ]
}

Response 201 Created:
{
    "id": "650e8400-e29b-41d4-a716-446655440007",
    "name": "Jazz Night 2024",
    "status": "DRAFT",
    "createdAt": "2024-01-15T14:30:00Z"
}
```

## 🛒 Order Endpoints

### Create Order
```http
POST /orders
Authorization: Bearer {token}
Content-Type: application/json

{
    "customerId": "550e8400-e29b-41d4-a716-446655440000",
    "items": [
        {
            "sectorId": "950e8400-e29b-41d4-a716-446655440004",
            "quantity": 2
        },
        {
            "sectorId": "950e8400-e29b-41d4-a716-446655440006",
            "quantity": 1
        }
    ]
}

Response 201 Created:
{
    "id": "150e8400-e29b-41d4-a716-446655440008",
    "orderNumber": "ORD-2024-000145",
    "customer": {
        "id": "550e8400-e29b-41d4-a716-446655440000",
        "name": "João Silva"
    },
    "items": [
        {
            "sector": "Pista Premium",
            "quantity": 2,
            "unitPrice": 180.00,
            "total": 360.00
        },
        {
            "sector": "Camarote VIP",
            "quantity": 1,
            "unitPrice": 350.00,
            "total": 350.00
        }
    ],
    "summary": {
        "subtotal": 710.00,
        "tax": 71.00,
        "total": 781.00
    },
    "status": "PENDING_PAYMENT",
    "expiresAt": "2024-01-15T15:00:00Z",
    "createdAt": "2024-01-15T14:45:00Z"
}
```

### Get Order Status
```http
GET /orders/{id}
Authorization: Bearer {token}

Response 200 OK:
{
    "id": "150e8400-e29b-41d4-a716-446655440008",
    "orderNumber": "ORD-2024-000145",
    "status": "PAID",
    "payment": {
        "method": "CREDIT_CARD",
        "status": "APPROVED",
        "paidAt": "2024-01-15T14:50:00Z"
    },
    "tickets": [
        {
            "id": "250e8400-e29b-41d4-a716-446655440009",
            "code": "TKT-RF24-001",
            "sector": "Pista Premium",
            "status": "VALID",
            "qrCode": "data:image/png;base64,..."
        },
        {
            "id": "250e8400-e29b-41d4-a716-446655440010",
            "code": "TKT-RF24-002",
            "sector": "Pista Premium",
            "status": "VALID",
            "qrCode": "data:image/png;base64,..."
        },
        {
            "id": "250e8400-e29b-41d4-a716-446655440011",
            "code": "TKT-RF24-003",
            "sector": "Camarote VIP",
            "status": "VALID",
            "qrCode": "data:image/png;base64,..."
        }
    ]
}
```

### List Customer Orders
```http
GET /orders?customerId={customerId}&status=PAID&page=1&size=10
Authorization: Bearer {token}

Response 200 OK:
{
    "content": [
        {
            "id": "150e8400-e29b-41d4-a716-446655440008",
            "orderNumber": "ORD-2024-000145",
            "event": {
                "id": "650e8400-e29b-41d4-a716-446655440001",
                "name": "Rock Festival 2024",
                "date": "2024-06-15T18:00:00Z"
            },
            "ticketCount": 3,
            "total": 781.00,
            "status": "PAID",
            "createdAt": "2024-01-15T14:45:00Z"
        }
    ],
    "page": {
        "size": 10,
        "number": 1,
        "totalElements": 23,
        "totalPages": 3
    }
}
```

## 💳 Payment Endpoints

### Process Payment
```http
POST /payments
Authorization: Bearer {token}
Content-Type: application/json

{
    "orderId": "150e8400-e29b-41d4-a716-446655440008",
    "method": "CREDIT_CARD",
    "card": {
        "number": "4111111111111111",
        "holder": "JOAO SILVA",
        "expiry": "12/25",
        "cvv": "123"
    }
}

Response 200 OK:
{
    "id": "350e8400-e29b-41d4-a716-446655440012",
    "orderId": "150e8400-e29b-41d4-a716-446655440008",
    "transactionId": "TXN-2024-000567",
    "status": "APPROVED",
    "method": "CREDIT_CARD",
    "amount": 781.00,
    "processedAt": "2024-01-15T14:50:00Z"
}
```

### Get Payment Methods
```http
GET /payments/methods

Response 200 OK:
{
    "methods": [
        {
            "code": "CREDIT_CARD",
            "name": "Cartão de Crédito",
            "icon": "https://cdn.vibraticket.com/icons/credit-card.svg",
            "enabled": true,
            "installments": {
                "min": 1,
                "max": 12,
                "interestFree": 3
            }
        },
        {
            "code": "PIX",
            "name": "PIX",
            "icon": "https://cdn.vibraticket.com/icons/pix.svg",
            "enabled": true,
            "discount": 5.0
        },
        {
            "code": "BOLETO",
            "name": "Boleto Bancário",
            "icon": "https://cdn.vibraticket.com/icons/boleto.svg",
            "enabled": true,
            "daysToExpire": 3
        }
    ]
}
```

## 📊 Reports Endpoints

### Sales Report
```http
GET /reports/sales?startDate=2024-01-01&endDate=2024-01-31&groupBy=day
Authorization: Bearer {admin_token}

Response 200 OK:
{
    "period": {
        "start": "2024-01-01",
        "end": "2024-01-31"
    },
    "summary": {
        "totalOrders": 1245,
        "totalRevenue": 458750.00,
        "averageOrderValue": 368.27,
        "ticketsSold": 3890
    },
    "data": [
        {
            "date": "2024-01-01",
            "orders": 42,
            "revenue": 15480.00,
            "tickets": 131
        },
        {
            "date": "2024-01-02",
            "orders": 38,
            "revenue": 13920.00,
            "tickets": 118
        }
        // ... more days
    ],
    "topEvents": [
        {
            "id": "650e8400-e29b-41d4-a716-446655440001",
            "name": "Rock Festival 2024",
            "ticketsSold": 890,
            "revenue": 142500.00
        }
    ]
}
```

### Event Performance
```http
GET /reports/events/{eventId}/performance
Authorization: Bearer {admin_token}

Response 200 OK:
{
    "event": {
        "id": "650e8400-e29b-41d4-a716-446655440001",
        "name": "Rock Festival 2024"
    },
    "sales": {
        "total": 35000,
        "percentage": 70.0,
        "byDay": [
            {
                "date": "2024-01-15",
                "quantity": 1200,
                "revenue": 192000.00
            }
        ]
    },
    "sectors": [
        {
            "name": "Pista Premium",
            "capacity": 20000,
            "sold": 14580,
            "percentage": 72.9,
            "revenue": 2624400.00
        }
    ]
}
```

## 🔍 Códigos de Status

| Código | Descrição | Uso |
|--------|-----------|-----|
| **200** | OK | Requisição bem-sucedida |
| **201** | Created | Recurso criado com sucesso |
| **204** | No Content | Requisição bem-sucedida sem conteúdo |
| **400** | Bad Request | Dados inválidos ou mal formatados |
| **401** | Unauthorized | Token ausente ou inválido |
| **403** | Forbidden | Sem permissão para o recurso |
| **404** | Not Found | Recurso não encontrado |
| **409** | Conflict | Conflito com o estado atual |
| **422** | Unprocessable Entity | Validação de negócio falhou |
| **429** | Too Many Requests | Rate limit excedido |
| **500** | Internal Server Error | Erro interno do servidor |
| **503** | Service Unavailable | Serviço temporariamente indisponível |

## ⚠️ Tratamento de Erros

### Formato de Erro Padrão
```json
{
    "error": {
        "code": "VALIDATION_ERROR",
        "message": "Validation failed",
        "timestamp": "2024-01-15T14:30:00Z",
        "path": "/api/v1/customers",
        "requestId": "550e8400-e29b-41d4-a716-446655440000",
        "details": [
            {
                "field": "email",
                "message": "Invalid email format",
                "code": "INVALID_FORMAT"
            },
            {
                "field": "document",
                "message": "CPF already registered",
                "code": "DUPLICATE_VALUE"
            }
        ]
    }
}
```

### Códigos de Erro Comuns

| Código | Descrição |
|--------|-----------|
| `VALIDATION_ERROR` | Erro de validação de dados |
| `NOT_FOUND` | Recurso não encontrado |
| `DUPLICATE_VALUE` | Valor duplicado (email, CPF, etc) |
| `INSUFFICIENT_STOCK` | Ingressos insuficientes |
| `PAYMENT_FAILED` | Falha no processamento do pagamento |
| `ORDER_EXPIRED` | Pedido expirou |
| `UNAUTHORIZED` | Não autorizado |
| `FORBIDDEN` | Sem permissão |
| `RATE_LIMITED` | Muitas requisições |

### Rate Limiting

```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1642338000

429 Too Many Requests
{
    "error": {
        "code": "RATE_LIMITED",
        "message": "Rate limit exceeded. Try again in 60 seconds",
        "retryAfter": 60
    }
}
```

---

<div align="center">

[← Modelo de Domínio](./domain-model.md) | [Próximo: Decisões Arquiteturais →](./decisions.md)

</div>