# Developer Setup

This guide explains how a developer can run the AuthService project locally.

## Why Docker Is Used

Docker is useful for both production and development.

In production, Docker packages the API into a consistent container that Render can build and run.

In development, Docker helps every developer use the same database, same PostgreSQL version, same ports, and same runtime behavior. This avoids common issues like:

- One developer has PostgreSQL installed and another does not.
- Different database versions cause different behavior.
- Local machine passwords and ports are different.
- New developers spend too much time setting up dependencies.

## Option 1: Run Everything With Docker Compose

This is the easiest setup for a new developer.

Requirements:

- Git
- Docker Desktop

Clone the repository:

```powershell
git clone https://github.com/Lakhanpal-1/authservice-clean-architecture.git
cd authservice-clean-architecture
```

Start the API and PostgreSQL:

```powershell
docker compose up --build
```

Open Swagger:

```text
http://localhost:10000/swagger
```

The compose file starts:

- `postgres` - local PostgreSQL database
- `api` - ASP.NET Core AuthService API

The API automatically runs EF Core migrations on startup, so the `Users` table is created automatically.

Stop the containers:

```powershell
docker compose down
```

Stop and delete the local database volume:

```powershell
docker compose down -v
```

## Option 2: Run API Locally With Installed PostgreSQL

Use this when you want to debug directly from Visual Studio, Rider, or VS Code.

Requirements:

- .NET 10 SDK
- PostgreSQL
- Git

Clone the repository:

```powershell
git clone https://github.com/Lakhanpal-1/authservice-clean-architecture.git
cd authservice-clean-architecture
```

Create local app settings:

```powershell
Copy-Item AuthService.API\appsettings.example.json AuthService.API\appsettings.Development.json
```

Edit `AuthService.API\appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=auth_db;Username=postgres;Password=your-local-password"
  },
  "Jwt": {
    "Key": "Local_Development_Jwt_Key_Change_This",
    "Issuer": "AuthService",
    "Audience": "HRSystem"
  }
}
```

Restore and build:

```powershell
dotnet restore AuthService.API\AuthService.API.csproj
dotnet build AuthService.API\AuthService.API.csproj
```

Run the API:

```powershell
dotnet run --project AuthService.API
```

Open the Swagger URL printed in the terminal, usually:

```text
https://localhost:<port>/swagger
```

## Branch Workflow

Use this branch flow:

- `main` - production branch deployed by Render.
- `main-redesign` - development branch for changes and experiments.

Recommended team workflow:

1. Create a feature branch from `main-redesign`.
2. Make changes.
3. Open a pull request into `main-redesign`.
4. Test locally.
5. Open a pull request from `main-redesign` into `main`.
6. GitHub Actions CI must pass.
7. Merge to `main`.
8. Render deploys production.

Pushing to `main-redesign` is safe for development checks because it runs CI without deploying production.

## Useful Docker Commands

Build image only:

```powershell
docker build -t authservice .
```

Run API image against an external database:

```powershell
docker run -p 10000:10000 `
  -e ConnectionStrings__DefaultConnection="Host=<host>;Port=5432;Database=postgres;Username=<user>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true" `
  -e Jwt__Key="Local_Or_Production_Strong_Jwt_Key" `
  -e Jwt__Issuer="AuthService" `
  -e Jwt__Audience="HRSystem" `
  authservice
```

List running containers:

```powershell
docker ps
```

View compose logs:

```powershell
docker compose logs -f
```

Rebuild after code changes:

```powershell
docker compose up --build
```

## Environment Variables

Production values are configured in Render, not committed to GitHub.

Required production variables:

```text
ConnectionStrings__DefaultConnection
Jwt__Key
Jwt__Issuer
Jwt__Audience
ASPNETCORE_ENVIRONMENT
```

Never commit real database passwords or JWT secrets.
