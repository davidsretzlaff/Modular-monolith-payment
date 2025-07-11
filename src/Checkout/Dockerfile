FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Shared.Core first
COPY Shared/Core ./Shared/Core

# Copy Checkout source code
COPY Checkout/Api ./Checkout/Api
COPY Checkout/Application ./Checkout/Application
COPY Checkout/Domain ./Checkout/Domain
COPY Checkout/Infrastructure ./Checkout/Infrastructure

# Build the application
WORKDIR "/src/Checkout/Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.Api.dll"] 