using Microsoft.Extensions.Logging;

namespace Modular.Charge.ChargeEngine.Metrics;

public class ChargeEngineMetrics
{
    private readonly ILogger<ChargeEngineMetrics> _logger;
    private readonly Dictionary<string, long> _counters = new();
    private readonly Dictionary<string, List<TimeSpan>> _timers = new();
    private readonly object _lock = new();

    public ChargeEngineMetrics(ILogger<ChargeEngineMetrics> logger)
    {
        _logger = logger;
    }

    public void IncrementCounter(string name, long value = 1)
    {
        lock (_lock)
        {
            if (!_counters.ContainsKey(name))
                _counters[name] = 0;
            
            _counters[name] += value;
        }

        _logger.LogDebug("Counter {CounterName} incremented by {Value}, total: {Total}", 
            name, value, _counters[name]);
    }

    public void RecordTimer(string name, TimeSpan duration)
    {
        lock (_lock)
        {
            if (!_timers.ContainsKey(name))
                _timers[name] = new List<TimeSpan>();
            
            _timers[name].Add(duration);
        }

        _logger.LogDebug("Timer {TimerName} recorded: {Duration}ms", 
            name, duration.TotalMilliseconds);
    }

    public long GetCounter(string name)
    {
        lock (_lock)
        {
            return _counters.TryGetValue(name, out var value) ? value : 0;
        }
    }

    public TimeSpan GetAverageTimer(string name)
    {
        lock (_lock)
        {
            if (!_timers.TryGetValue(name, out var timers) || !timers.Any())
                return TimeSpan.Zero;

            var averageTicks = timers.Average(t => t.Ticks);
            return TimeSpan.FromTicks((long)averageTicks);
        }
    }

    public void LogMetrics()
    {
        lock (_lock)
        {
            _logger.LogInformation("=== Charge Engine Metrics ===");
            
            foreach (var counter in _counters)
            {
                _logger.LogInformation("Counter {Name}: {Value}", counter.Key, counter.Value);
            }

            foreach (var timer in _timers)
            {
                var average = GetAverageTimer(timer.Key);
                var count = timer.Value.Count;
                _logger.LogInformation("Timer {Name}: {Count} calls, avg: {Average}ms", 
                    timer.Key, count, average.TotalMilliseconds);
            }
        }
    }

    public void Reset()
    {
        lock (_lock)
        {
            _counters.Clear();
            _timers.Clear();
        }

        _logger.LogInformation("Metrics reset");
    }
} 