# Free Hosting and CI/CD

This repository is ready for a free or low-cost Docker deployment with GitHub-based CI/CD.

## Recommended Free Setup

Use this path for the simplest free setup:

- GitHub repository: source code, pull requests, CI checks, Docker image publishing.
- Render Free Web Service: hosts the ASP.NET Core API from the Dockerfile.
- Supabase Free Postgres: provides the PostgreSQL database.

Render supports Docker web services and automatic deploys from GitHub. Render's free web services can spin down when idle, and Render's own free Postgres expires after a limited time, so Supabase is a better free database choice for a demo or portfolio deployment.

## Branch Flow

- `main-redesign`: development branch for redesign or feature work.
- Pull request from `main-redesign` into `main`: CI must pass before merge.
- `main`: production branch.
- Push or merge to `main`: Docker image is built and published to GitHub Container Registry.
- Successful image publish: Render deploy hook can deploy production automatically.

## GitHub Actions

The repository includes:

- `.github/workflows/ci.yml` - restores, builds, and publish-checks the API.
- `.github/workflows/docker-publish.yml` - builds and publishes a Docker image to GHCR on `main`.
- `.github/workflows/deploy-render.yml` - triggers Render production deployment after the image publish succeeds.

## Render Web Service

Create a Render Web Service:

1. Go to Render Dashboard.
2. Create a new Web Service.
3. Connect the GitHub repository.
4. Select Docker as the runtime.
5. Select branch `main`.
6. Use the repository Dockerfile.
7. Set the service plan to Free.

The Dockerfile exposes port `10000`, which matches Render's default web service port.

## Supabase Database

Create a Supabase project and copy the PostgreSQL connection string.

In Render, add these environment variables:

```text
ConnectionStrings__DefaultConnection=<supabase-postgres-connection-string>
Jwt__Key=<strong-random-secret-at-least-32-characters>
Jwt__Issuer=AuthService
Jwt__Audience=HRSystem
ASPNETCORE_ENVIRONMENT=Production
```

## Automatic Production Deploy

For deploy hooks:

1. In Render, open the production web service.
2. Create or copy the Deploy Hook URL.
3. In GitHub, open repository Settings.
4. Go to Secrets and variables > Actions.
5. Add repository secret:

```text
RENDER_DEPLOY_HOOK_URL=<render-deploy-hook-url>
```

After this, every successful production Docker image publish can trigger a Render deploy.

## Production Checklist

- Enable GitHub branch protection for `main`.
- Require pull requests before merging.
- Require the `CI` workflow to pass.
- Keep secrets only in Render and GitHub Actions secrets.
- Run EF Core migrations against the production database before first use.
