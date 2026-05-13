# AutoriaStore API

API REST de e-commerce construída com **.NET 10** seguindo os princípios de **Clean Architecture** e **Domain-Driven Design**.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-13-239120?style=flat-square&logo=csharp)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14-4169E1?style=flat-square&logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-compose-2496ED?style=flat-square&logo=docker)

---

## Sobre o projeto

AutoriaStore é uma API REST para gestão de uma loja virtual. Oferece operações sobre usuários, produtos, categorias, pedidos e pagamentos via PIX, com suporte a upload de imagens e cálculo de frete.

O projeto foi desenvolvido como exercício de arquitetura de software, priorizando separação de responsabilidades, testabilidade e código orientado ao domínio do negócio.

---

## Stack

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Banco de dados | PostgreSQL 14 |
| Autenticação | JWT (RS256) |
| Object Storage | MinIO (local) / Cloudflare R2 (prod) |
| E-mail | Resend |
| Pagamentos | AbacatePay (PIX) |
| Frete | API Correios |
| Containers | Docker + Docker Compose |

---

## Arquitetura

O projeto segue **Clean Architecture** com quatro camadas. A direção de dependência aponta sempre para dentro:

```
API  ──►  Application  ──►  Domain
                ▲
          Infrastructure
```

| Camada | Responsabilidade |
|---|---|
| `Domain` | Entidades, enums, regras de negócio, contratos de repositório |
| `Application` | Casos de uso, DTOs de aplicação, contratos de serviço |
| `API` | Controllers, DTOs de transporte, presenters, middlewares |
| `Infrastructure` | EF Core, repositórios, serviços externos, injeção de dependência |

Cada caso de uso representa uma única intenção de negócio e é facilmente testável em isolamento.

---

## Funcionalidades

### Usuários
- Cadastro de clientes
- Login / Logout com cookie HttpOnly
- Buscar, listar, atualizar e remover usuários
- Controle de roles (`Admin`, `Customer`)

### Produtos
- CRUD completo de produtos e categorias
- Visões distintas para cliente e administrador
- Upload e gerenciamento de imagens de produtos

### Pedidos
- Criação de pedidos com múltiplos itens
- Consulta de pedido por ID
- Controle de status do pedido

### Pagamentos
- Geração de cobrança PIX via **AbacatePay**
- Recebimento e validação de webhook de pagamento
- Idempotência nas transações

### Frete
- Cálculo de frete via **API dos Correios**

---

## Estrutura de pastas

```
src/
  API/
    Controllers/       # Um controller por caso de uso
    Dtos/              # Modelos de request/response HTTP
    Presenters/        # Mapeamento Application → API
    Handlers/          # Tratamento global de exceções

  Application/
    UseCases/          # Casos de uso organizados por funcionalidade
    Dtos/              # DTOs de entrada/saída da camada de aplicação
    Exceptions/        # Exceções de domínio da aplicação
    Interfaces/        # Contratos de serviços externos

  Domain/
    Entities/          # Entidades e agregados
    Enums/             # Enums de domínio
    Interfaces/        # Contratos de repositórios

  Infrastructure/
    Context/           # DbContext do EF Core
    EntitiesConfiguration/ # Configurações Fluent API
    Repositories/      # Implementações dos repositórios
    Services/          # Serviços externos (JWT, S3, e-mail, pagamento)
    Migrations/        # Migrações do banco de dados

tests/
  UnitTests/
    UseCases/          # Testes dos casos de uso
```

---

## Variáveis de ambiente

Copie `.env.example` para `.env` dentro de `src/API/` e preencha os valores:

```bash
cp src/API/.env.example src/API/.env
```

| Variável | Descrição |
|---|---|
| `DB_HOST`, `DB_PORT`, `DB_DATABASE`, `DB_USER`, `DB_PASSWORD` | Conexão com PostgreSQL |
| `ORIGIN` | URL permitida pelo CORS |
| `JWT_ISSUER`, `JWT_AUDIENCE`, `JWT_EXPIRATION_TIME_IN_MINUTES` | Configurações do token JWT |
| `JWT_PRIVATE_KEY`, `JWT_PUBLIC_KEY` | Par de chaves RSA para assinatura JWT |
| `AUTH_TOKEN_COOKIE` | Nome do cookie de autenticação |
| `OBJECT_STORAGE_BUCKET`, `OBJECT_STORAGE_ENDPOINT`, `OBJECT_STORAGE_ACCESS_KEY`, `OBJECT_STORAGE_SECRET_KEY`, `OBJECT_STORAGE_PUBLIC_URL`, `OBJECT_STORAGE_ID` | Configurações do MinIO / Cloudflare R2 |
| `RESEND_API_KEY`, `RESEND_FROM_EMAIL` | Credenciais do serviço de e-mail Resend |
| `ABACATE_PAY_API_KEY`, `ABACATE_PAY_WEBHOOK_SECRET`, `ABACATE_PAY_WEBHOOK_PUBLIC_KEY` | Credenciais do AbacatePay |

---

## Rodando localmente

### Pré-requisitos

- [Docker](https://www.docker.com/) e Docker Compose
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (para rodar fora do container)

### 1. Configurar variáveis de ambiente

```bash
cp src/API/.env.example src/API/.env
# edite src/API/.env com seus valores
```

### 2. Criar a rede Docker

```bash
docker network create user-crud-app-network
```

### 3. Subir os serviços

```bash
docker compose up -d
```

Isso sobe: **API** (porta `8082`), **PostgreSQL** (porta `5555`) e **MinIO** (porta `9000` / console `9001`).

### 4. Aplicar as migrações

```bash
docker compose exec api dotnet ef database update \
  --project src/Infrastructure \
  --startup-project src/API
```

A API estará disponível em `http://localhost:8082`.

---

## Testes

```bash
dotnet test tests/UnitTests
```

Os testes cobrem os casos de uso da camada `Application`, com repositórios e serviços externos mockados.
