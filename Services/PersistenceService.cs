using Microsoft.Extensions.Logging;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Services;

public class PersistenceService
{
  private const string MetricsKey = "metrics";
  private const string MetricValuesKey = "metricvalues";
  private readonly bool _enabled;
  private readonly IMoBroPersistenceManager _persistenceManager;
  private readonly IMoBroService _service;
  private readonly ILogger _logger;

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
  }


  public void LoadMetrics()
  {
    if (!_enabled) return;

    lock (this)
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

  public void PersistMetrics()
  {
    if (!_enabled) return;

    lock (this)
    {
      var metrics = _service.GetAll<Metric>().ToArray();
      var metricValues = _service.GetMetricValues().ToArray();
      _persistenceManager.Put(MetricsKey, metrics);
      _persistenceManager.Put(MetricValuesKey, metricValues);
    }
  }
}