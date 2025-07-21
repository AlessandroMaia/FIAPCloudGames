<h1 align="center">
  <img src="images/logo.png" alt="FCG" />
</h1>

<div align="center">

![image](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![image](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![image](https://img.shields.io/badge/Docker%20Compose-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![image](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=Swagger&logoColor=white)
![image](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white)
![image](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=white)

</div>

<div align="center">

[![English (US)](https://raw.githubusercontent.com/gosquared/flags/master/flags/flags/shiny/32/United-States.png)](./README.en.md)
[![Português (BR)](https://raw.githubusercontent.com/gosquared/flags/master/flags/flags/shiny/32/Brazil.png)](./README.md)

</div>

<div align="center">

[Objetivos](#-objetivos)&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
[Funcionalidades](#-funcionalidades)&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
[Autenticação e Segurança](#-autenticação-e-segurança)&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
[Tecnologias](#-tecnologias)&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
[Documentação da API](#-documentação-da-api)&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
[Como Executar](#️-como-executar)

</div>

---

## 🎯 Objetivos

O objetivo é construir uma **API REST com .NET 9** para gerenciar usuários e seus jogos adquiridos, servindo como base para funcionalidades futuras como matchmaking e orquestração de servidores de jogos.

Este MVP permite à FIAP validar a arquitetura proposta, garantindo segurança, escalabilidade e boas práticas desde a primeira versão.

---

## 🚀 Funcionalidades

### Cadastro de Usuários
- Registro com nome, e-mail e senha segura
- Validação de formato e força da senha

### Autenticação e Autorização
- Autenticação via **JWT armazenado em cookies `HttpOnly`**
- Níveis de acesso:
  - Usuário comum
  - Administrador (acesso a recursos privilegiados)

### Gerenciamento
- Cadastro e listagem de jogos (admin)
- Biblioteca de jogos por usuário autenticado
- Estrutura preparada para campanhas e promoções

---

## 🔐 Autenticação e Segurança

A API utiliza **JWT armazenado em cookies `HttpOnly`**, protegendo os tokens contra ataques XSS. O sistema também implementa:

- **Refresh Tokens**: renovação de sessão sem novo login
- **RBAC**: Controle de acesso baseado em papéis (`Admin`, `User`)
- **SignalR**: Logout forçado em tempo real, ideal para revogação de sessão
- **Middlewares customizados**: tratamento global de erros e logging estruturado

---

## 🛠 Tecnologias

- [.NET 9](https://dotnet.microsoft.com/)
- **Entity Framework Core**
- **JWT + Refresh Token**
- **SignalR** (WebSockets)
- **Swagger/OpenAPI**
- **FluentValidation**
- **Serilog + Seq** (observabilidade)
- **Docker Compose**
- **PostgreSQL**

---

## 📚 Documentação da API

- Swagger: [`http://localhost:5000/swagger`](http://localhost:5000/swagger)  
- Seq (logs): [`http://localhost:8081`](http://localhost:8081)

---

## ⚙️ Como Executar

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- CLI EF Core (`dotnet-ef`)

### 🔧 Configuração de Ambiente

Edite o arquivo `appsettings.Development.json` com os dados abaixo para configurar conexão com o banco, JWT e logging:

```json
{
  "ConnectionStrings": {
    "Database": "Host=postgres;Port=5432;Database=fcg;Username=root;Password=root;Include Error Detail=true"
  },
  "Jwt": {
    "Secret": "cc95adbf68f11e9bcb0274d826dfcfc88b790c50",
    "Issuer": "fiap-cloud-games",
    "Audience": "developers",
    "ExpirationInMinutes": 10
  },
  "AuthKeys": {
    "AccessToken": "access_token",
    "RefreshToken": "refresh_token"
  },
  "CORsWhitelistedDomains": [
    "http://localhost:3000"
  ],
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Seq"],
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Seq",
        "Args": { "ServerUrl": "http://seq:5341" }
      }
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": { "Microsoft": "Information" }
    },
    "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"]
  }
}
```

💡 **Importante**: o host postgres deve ser usado apenas quando o banco estiver sendo executado via Docker Compose. Para executar comandos de migration via CLI local, altere temporariamente o host para localhost.

### ▶️ Inicializando o Sistema

Você pode iniciar o sistema de duas formas:

#### ✅ Opção 1: Via Docker Compose (recomendado)

```bash
docker-compose up --build
```
Certifique-se de que o `docker-compose` esteja definido como projeto de inicialização no Visual Studio, ou execute o comando acima via terminal.

A API estará disponível em:

- `http://localhost:5000/swagger`
- `http://localhost:8081` (visualização dos logs via Seq)

#### 🛠️ Opção 2: Execução Manual

1. Inicie o PostgreSQL manualmente (local ou via Docker)
2. Altere a string de conexão para usar `localhost`
3. Aplique as migrations:
```bash
dotnet ef database update
```
4. Rode a aplicação:
```bash
dotnet run --project src/Fcg.Api
```
💡 **Importante**: Revise as variáveis de conexão com o banco de dados dentro do arquivo docker-compose