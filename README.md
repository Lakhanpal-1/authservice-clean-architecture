# AuthService Microservice

AuthService is a clean-architecture ASP.NET Core authentication microservice for an HR system. It provides user registration and login APIs, PostgreSQL persistence, password hashing, JWT token generation, centralized exception handling, and Swagger documentation for local development.

## Architecture

The solution is organized into focused layers:

- `AuthService.API` - ASP.NET Core Web API, controllers, middleware, Swagger, and application startup.
- `AuthService.Application` - login/register use cases, commands, and service contracts.
- `AuthService.Domain` - user entity, roles, and domain exceptions.
- `AuthService.Infrastructure` - Entity Framework Core, PostgreSQL persistence, repositories, password hashing, JWT generation, and seed data.
- `AuthService.Contracts` - request and response DTOs exposed by the API.

## Features

- User registration with hashed passwords
- User login with JWT access tokens
- PostgreSQL database integration using Entity Framework Core
- Clean architecture with dependency injection between layers
- Development Swagger UI
- Admin seed support for local development
- Docker-ready deployment setup

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT Bearer Authentication
- BCrypt password hashing
- Swagger / OpenAPI

## Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL

### Configuration

Copy the example settings and replace the placeholder values:

```powershell
Copy-Item AuthService.API\appsettings.example.json AuthService.API\appsettings.Development.json
```

Required settings:

- `ConnectionStrings:DefaultConnection`
- `Jwt:Key`
- `Jwt:Issuer`
- `Jwt:Audience`

For production, provide these values through environment variables or your hosting provider's secret manager.

### Run Locally

```powershell
dotnet restore AuthService.API\AuthService.API.csproj
dotnet ef database update --project AuthService.Infrastructure --startup-project AuthService.API
dotnet run --project AuthService.API
```

Swagger is available at:

```text
https://localhost:<port>/swagger
```

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| `POST` | `/api/auth/register` | Register a new user |
| `POST` | `/api/auth/login` | Authenticate a user and return a JWT |

## Docker Deployment

Build the container image:

```powershell
docker build -t authservice .
```

Run the API:

```powershell
docker run -p 8080:8080 `
  -e ConnectionStrings__DefaultConnection="Host=<host>;Port=5432;Database=auth_db;Username=<user>;Password=<password>" `
  -e Jwt__Key="<strong-secret-key>" `
  -e Jwt__Issuer="AuthService" `
  -e Jwt__Audience="HRSystem" `
  authservice
```

The API will be available at:

```text
http://localhost:8080/swagger
```

## Deployment Notes

This service can be deployed to platforms that support Docker or .NET apps, such as Azure App Service, Render, Railway, Fly.io, or a VPS. Use managed PostgreSQL for production, configure secrets in the platform dashboard, and run EF Core migrations during release or startup automation.

## Repository Owner

Maintained by Aman Lakhanpal.
