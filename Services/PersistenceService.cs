using Microsoft.Extensions.Logging;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Services;

public class PersistenceService : IDisposable
{
  private const string MetricsKey = "metrics";
  private const string MetricValuesKey = "metricvalues";
  private static readonly TimeSpan PersistDelay = TimeSpan.FromSeconds(5);

  private readonly bool _enabled;
  private readonly IMoBroPersistenceManager _persistenceManager;
  private readonly IMoBroService _service;
  private readonly ILogger _logger;
  private readonly Lock _lock = new();
  private readonly Timer _persistTimer;
  private int _persistScheduled;

  public PersistenceService(
    IMoBroPersistenceManager persistenceManager,
    IMoBroService service,
    IMoBroSettings settings,
    ILogger logger
  )
  {
    _persistenceManager = persistenceManager;
    _service = service;
    _logger = logger;
    _enabled = settings.GetValue("persistence_enable", true);
    _persistTimer = new Timer(_ => PersistScheduled(), null, Timeout.Infinite, Timeout.Infinite);
  }


  public void LoadMetrics()
  {
    if (!_enabled) return;

    lock (_lock)
    {
      try
      {
        var metrics = _persistenceManager.Get<Metric[]>(MetricsKey);
        if (metrics is { Length: > 0 }) _service.Register(metrics);
      }
      catch (Exception e)
      {
        _logger.LogWarning(e, "Failed to load persisted metrics");
        _persistenceManager.Remove(MetricsKey);
      }

      try
      {
        var metricValues = _persistenceManager.Get<MetricValue[]>(MetricValuesKey);
        if (metricValues is { Length: > 0 }) _service.UpdateMetricValues(metricValues);
      }
      catch (Exception e)
      {
        _logger.LogWarning(e, "Failed to load persisted metric values");
        _persistenceManager.Remove(MetricValuesKey);
      }
    }
  }

  /// <summary>
  /// Persists the metrics after a short delay. Multiple calls within the delay are combined into a single write,
  /// so frequent value updates do not cause a write per request.
  /// </summary>
  public void SchedulePersist()
  {
    if (!_enabled) return;
    if (Interlocked.Exchange(ref _persistScheduled, 1) == 1) return;
    _persistTimer.Change(PersistDelay, Timeout.InfiniteTimeSpan);
  }

  private void PersistScheduled()
  {
    Interlocked.Exchange(ref _persistScheduled, 0);
    try
    {
      PersistMetrics();
    }
    catch (Exception e)
    {
      _logger.LogWarning(e, "Failed to persist metrics");
    }
  }

  public void PersistMetrics()
  {
    if (!_enabled) return;

    lock (_lock)
    {
      var metrics = _service.GetAll<Metric>().ToArray();
      var metricValues = _service.GetMetricValues().ToArray();
      _persistenceManager.Put(MetricsKey, metrics);
      _persistenceManager.Put(MetricValuesKey, metricValues);
    }
  }

  public void Dispose()
  {
    _persistTimer.Dispose();
  }
}