using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Charge.ChargeEngine.Configuration;
using Charge.ChargeEngine.Services;
using Charge.ChargeEngine.Metrics;
using Charge.Domain.Repositories;
using Charge.Infrastructure.Repositories;
using Checkout.Domain.Repositories;
using Checkout.Infrastructure.Repositories;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Configuration
        services.Configure<ChargeEngineOptions>(
            context.Configuration.GetSection(ChargeEngineOptions.SectionName));

        // Repositories
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        // Services
        services.AddScoped<IPaymentGateway, FakePaymentGateway>();
        services.AddScoped<BatchProcessingService>();
        services.AddSingleton<ChargeEngineMetrics>();

        // Background Services
        services.AddHostedService<ChargeEngineService>();
    })
    .Build();

await host.RunAsync(); 