FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Shared.Core first
COPY Shared/Core ./Shared/Core

# Copy Admin source code
COPY Admin/Api ./Admin/Api
COPY Admin/Application ./Admin/Application
COPY Admin/Domain ./Admin/Domain
COPY Admin/Infrastructure ./Admin/Infrastructure

# Build the application
WORKDIR "/src/Admin/Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Admin.Api.dll"] 