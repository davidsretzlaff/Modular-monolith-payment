using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Modular.Charge.ChargeEngine.Configuration;
using Modular.Charge.ChargeEngine.Services;
using Modular.Charge.ChargeEngine.Metrics;
using Modular.Charge.Domain.Repositories;
using Modular.Charge.Domain.Services;
using Modular.Charge.Infrastructure.Repositories;

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