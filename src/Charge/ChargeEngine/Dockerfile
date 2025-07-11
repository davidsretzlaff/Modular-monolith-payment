FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Shared.Core first
COPY Shared/Core ./Shared/Core

# Copy Charge modules
COPY Charge/Domain ./Charge/Domain
COPY Charge/Application ./Charge/Application
COPY Charge/Infrastructure ./Charge/Infrastructure

# Copy Checkout modules (for IPaymentGateway)
COPY Checkout/Domain ./Checkout/Domain
COPY Checkout/Infrastructure ./Checkout/Infrastructure

# Copy ChargeEngine source code
COPY Charge/ChargeEngine ./Charge/ChargeEngine

# Build the application
WORKDIR "/src/Charge/ChargeEngine"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChargeEngine.dll"] 