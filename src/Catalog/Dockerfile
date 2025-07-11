FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Shared.Core first
COPY Shared/Core ./Shared/Core

# Copy Catalog source code
COPY Catalog/Api ./Catalog/Api
COPY Catalog/Application ./Catalog/Application
COPY Catalog/Domain ./Catalog/Domain
COPY Catalog/Infrastructure ./Catalog/Infrastructure

# Build the application
WORKDIR "/src/Catalog/Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"] 