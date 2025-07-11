FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Shared.Core first
COPY Shared/Core ./Shared/Core

# Copy Charge source code
COPY Charge/Api ./Charge/Api
COPY Charge/Application ./Charge/Application
COPY Charge/Domain ./Charge/Domain
COPY Charge/Infrastructure ./Charge/Infrastructure

# Build the application
WORKDIR "/src/Charge/Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Charge.Api.dll"] 