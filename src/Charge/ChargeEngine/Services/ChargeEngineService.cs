using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modular.Charge.ChargeEngine.Configuration;
using Modular.Charge.ChargeEngine.Services;
using Modular.Charge.Domain.Repositories;

namespace Modular.Charge.ChargeEngine.Services;

public class ChargeEngineService : BackgroundService
{
    private readonly ILogger<ChargeEngineService> _logger;
    private readonly ISaleRepository _saleRepository;
    private readonly BatchProcessingService _batchProcessingService;
    private readonly ChargeEngineOptions _options;

    public ChargeEngineService(
        ILogger<ChargeEngineService> logger,
        ISaleRepository saleRepository,
        BatchProcessingService batchProcessingService,
        IOptions<ChargeEngineOptions> options)
    {
        _logger = logger;
        _saleRepository = saleRepository;
        _batchProcessingService = batchProcessingService;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Charge Engine Service started with configuration: MaxConcurrency={MaxConcurrency}, BatchSize={BatchSize}, PollingInterval={PollingInterval}",
            _options.MaxConcurrency, _options.BatchSize, _options.PollingInterval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingSalesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Charge Engine Service is stopping...");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in charge engine main loop");
            }

            await Task.Delay(_options.PollingInterval, stoppingToken);
        }

        _logger.LogInformation("Charge Engine Service stopped");
    }

    private async Task ProcessPendingSalesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var pendingSales = await _saleRepository.GetPendingSalesAsync();
            
            if (!pendingSales.Any())
            {
                _logger.LogDebug("No pending sales to process");
                return;
            }

            _logger.LogInformation("Found {Count} pending sales to process", pendingSales.Count());

            // Process sales in batches
            await _batchProcessingService.ProcessBatchAsync(pendingSales, cancellationToken);

            _logger.LogInformation("Completed processing batch of {Count} sales", pendingSales.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing pending sales");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Charge Engine Service...");
        
        // Allow time for current processing to complete
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        
        await base.StopAsync(cancellationToken);
    }
} 