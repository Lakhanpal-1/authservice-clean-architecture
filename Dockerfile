FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AuthService.API/AuthService.API.csproj AuthService.API/
COPY AuthService.Application/AuthService.Application.csproj AuthService.Application/
COPY AuthService.Contracts/AuthService.Contracts.csproj AuthService.Contracts/
COPY AuthService.Domain/AuthService.Domain.csproj AuthService.Domain/
COPY AuthService.Infrastructure/AuthService.Infrastructure.csproj AuthService.Infrastructure/
RUN dotnet restore AuthService.API/AuthService.API.csproj

COPY . .
RUN dotnet publish AuthService.API/AuthService.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuthService.API.dll"]
