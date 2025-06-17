using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modular.Charge.ChargeEngine.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<ChargeEngineService>();
    })
    .Build();

await host.RunAsync(); 