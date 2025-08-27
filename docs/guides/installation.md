# 📦 Guia de Instalação

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-2019+-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

</div>

## 📑 Índice

- [📋 Pré-requisitos](#-pré-requisitos)
- [🔧 Configuração do Ambiente](#-configuração-do-ambiente)
- [🗄️ Configuração do Banco de Dados](#️-configuração-do-banco-de-dados)
- [🚀 Executando a Aplicação](#-executando-a-aplicação)
- [🐳 Docker Setup](#-docker-setup)
- [⚡ Instalação Rápida](#-instalação-rápida)
- [🔍 Verificação](#-verificação)
- [❓ Troubleshooting](#-troubleshooting)

## 📋 Pré-requisitos

### Software Necessário

| Software | Versão Mínima | Verificar Instalação |
|----------|---------------|---------------------|
| **.NET SDK** | 8.0 | `dotnet --version` |
| **SQL Server** | 2019 | `sqlcmd -?` |
| **Node.js** | 18.0 | `node --version` |
| **Git** | 2.30 | `git --version` |

### Software Opcional

| Software | Uso | Verificar Instalação |
|----------|-----|---------------------|
| **Docker** | Containers | `docker --version` |
| **Visual Studio** | IDE | - |
| **VS Code** | Editor | `code --version` |

### Requisitos de Sistema

- **OS**: Windows 10+, macOS 10.15+, Ubuntu 20.04+
- **RAM**: Mínimo 4GB (8GB recomendado)
- **Espaço**: 2GB livres
- **CPU**: 2 cores (4 cores recomendado)

## 🔧 Configuração do Ambiente

### 1. Instalação do .NET 8 SDK

#### Windows
```powershell
# Usando winget
winget install Microsoft.DotNet.SDK.8

# Ou baixe do site oficial
Start-Process https://dotnet.microsoft.com/download/dotnet/8.0
```

#### macOS
```bash
# Usando Homebrew
brew install --cask dotnet-sdk

# Ou baixe do site oficial
open https://dotnet.microsoft.com/download/dotnet/8.0
```

#### Linux (Ubuntu/Debian)
```bash
# Adicionar repositório Microsoft
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update

# Instalar SDK
sudo apt install dotnet-sdk-8.0
```

### 2. Instalação do SQL Server

#### Opção A: SQL Server Local

**Windows**
```powershell
# Download SQL Server Developer Edition
Start-Process https://www.microsoft.com/sql-server/sql-server-downloads
```

**macOS/Linux - Usar Docker**
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" \
  -p 1433:1433 --name sql-server \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

#### Opção B: Azure SQL (Cloud)
1. Crie uma conta no [Azure Portal](https://portal.azure.com)
2. Crie um Azure SQL Database
3. Configure o firewall para seu IP
4. Copie a connection string

### 3. Instalação de Ferramentas Adicionais

```bash
# Entity Framework CLI Tools
dotnet tool install --global dotnet-ef

# Verificar instalação
dotnet ef --version

# SQL Server CLI (opcional)
npm install -g sql-cli
```

## 🗄️ Configuração do Banco de Dados

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/vibraticket.git
cd vibraticket
```

### 2. Configure a Connection String

```bash
# Copie o template de configuração
cp src/Api/appsettings.Development.template.json src/Api/appsettings.Development.json
```

Edite `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=VibraTicket;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### 3. Crie o Banco de Dados

```bash
# Navegue para o diretório da solução
cd vibraticket

# Execute as migrações
dotnet ef database update -p src/Infrastructure -s src/Api

# Ou usando o script
./scripts/setup-database.sh
```

### 4. (Opcional) Popular Dados de Teste

```bash
# Execute o seed
dotnet run --project src/Api -- --seed

# Ou via SQL
sqlcmd -S localhost -U sa -P YourStrong@Password123 -d VibraTicket -i scripts/seed-data.sql
```

## 🚀 Executando a Aplicação

### Método 1: Command Line

```bash
# Na raiz do projeto
dotnet run --project src/Api

# Ou navegue até o projeto
cd src/Api
dotnet run
```

### Método 2: Visual Studio

1. Abra `VibraTicket.sln`
2. Defina `Api` como projeto de inicialização
3. Pressione `F5` ou clique em "Run"

### Método 3: VS Code

1. Abra a pasta do projeto
2. Pressione `F5`
3. Selecione ".NET Core" se solicitado

### Acessando a Aplicação

Após iniciar, acesse:
- **API**: https://localhost:7001
- **Swagger**: https://localhost:7001/swagger
- **Health Check**: https://localhost:7001/health

## 🐳 Docker Setup

### 1. Docker Compose (Recomendado)

```yaml
# docker-compose.yml
version: '3.8'

services:
  sql-server:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password123
    ports:
      - "1433:1433"
    volumes:
      - sql-data:/var/opt/mssql

  api:
    build:
      context: .
      dockerfile: src/Api/Dockerfile
    ports:
      - "7001:80"
      - "7002:443"
    depends_on:
      - sql-server
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=sql-server;Database=VibraTicket;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True

volumes:
  sql-data:
```

Executar:
```bash
# Build e start
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar
docker-compose down
```

### 2. Dockerfile Individual

```dockerfile
# src/Api/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/Api/Api.csproj"
COPY . .
WORKDIR "/src/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
```

## ⚡ Instalação Rápida

### Script para Windows

```powershell
# install.ps1
Write-Host "🚀 VibraTicket Quick Setup" -ForegroundColor Green

# Check .NET
if (!(dotnet --version)) {
    Write-Host "Installing .NET 8..." -ForegroundColor Yellow
    winget install Microsoft.DotNet.SDK.8
}

# Check Docker
if (!(docker --version)) {
    Write-Host "Docker not found. Please install Docker Desktop" -ForegroundColor Red
    Start-Process https://www.docker.com/products/docker-desktop
    exit
}

# Clone and setup
git clone https://github.com/seu-usuario/vibraticket.git
cd vibraticket

# Start with Docker Compose
docker-compose up -d

Write-Host "✅ Setup complete! Access https://localhost:7001/swagger" -ForegroundColor Green
```

### Script para Linux/macOS

```bash
#!/bin/bash
# install.sh

echo "🚀 VibraTicket Quick Setup"

# Check .NET
if ! command -v dotnet &> /dev/null; then
    echo "Installing .NET 8..."
    if [[ "$OSTYPE" == "darwin"* ]]; then
        brew install --cask dotnet-sdk
    else
        wget https://dot.net/v1/dotnet-install.sh
        chmod +x dotnet-install.sh
        ./dotnet-install.sh --channel 8.0
    fi
fi

# Check Docker
if ! command -v docker &> /dev/null; then
    echo "Docker not found. Please install Docker"
    exit 1
fi

# Clone and setup
git clone https://github.com/seu-usuario/vibraticket.git
cd vibraticket

# Start with Docker Compose
docker-compose up -d

echo "✅ Setup complete! Access https://localhost:7001/swagger"
```

## 🔍 Verificação

### 1. Verificar Instalação

```bash
# Verificar todas as dependências
dotnet --info
dotnet ef --version
docker --version
docker-compose --version
```

### 2. Verificar Serviços

```bash
# API Health Check
curl https://localhost:7001/health

# Swagger UI
open https://localhost:7001/swagger

# Database Connection
dotnet run --project src/Api -- --check-db
```

### 3. Executar Testes

```bash
# Unit Tests
dotnet test tests/Domain.Tests

# Integration Tests
dotnet test tests/Integration.Tests

# All Tests
dotnet test
```

## ❓ Troubleshooting

### Problema: Connection Refused ao SQL Server

```bash
# Verificar se SQL Server está rodando
docker ps | grep sql-server

# Ver logs do SQL Server
docker logs sql-server

# Testar conexão
sqlcmd -S localhost,1433 -U sa -P YourStrong@Password123 -Q "SELECT @@VERSION"
```

### Problema: Porta já em uso

```bash
# Windows
netstat -ano | findstr :7001

# Linux/macOS
lsof -i :7001

# Matar processo
kill -9 [PID]
```

### Problema: Certificado SSL

```json
// Adicione ao appsettings.json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:7000"
      },
      "Https": {
        "Url": "https://localhost:7001",
        "Certificate": {
          "Path": "localhost.pfx",
          "Password": "password"
        }
      }
    }
  }
}
```

### Problema: Migrations Falhando

```bash
# Remover migrations existentes
rm -rf src/Infrastructure/Migrations

# Criar nova migration
dotnet ef migrations add Initial -p src/Infrastructure -s src/Api

# Aplicar com verbose
dotnet ef database update -p src/Infrastructure -s src/Api --verbose
```

### Problema: Docker Build Falhando

```bash
# Limpar cache Docker
docker system prune -a

# Build com no-cache
docker-compose build --no-cache

# Ver logs detalhados
docker-compose up --build
```

## 📞 Suporte

Se continuar com problemas:

1. Verifique os [Issues conhecidos](https://github.com/seu-usuario/vibraticket/issues)
2. Consulte o [FAQ](../FAQ.md)
3. Abra um [novo issue](https://github.com/seu-usuario/vibraticket/issues/new)
4. Entre no [Slack](https://vibraticket.slack.com)

---

<div align="center">

[← Guias](./README.md) | [Próximo: Desenvolvimento →](./development.md)

</div>