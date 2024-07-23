using FastEndpoints;
using Microsoft.Extensions.Logging;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class DeleteMetricEndpoint(IMoBroService moBroService, ILogger logger) : EndpointWithoutRequest
{
  public override void Configure()
  {
    Delete("/metrics/{id}");
    Version(1);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Unregister a metric";
      s.Params["id"] = "The id of the metric to unregister";
      s.Responses[204] = "The metric was successfully unregistered";
      s.Responses[404] = "The metric does not exist";
    });
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var metricId = Route<string>("id");
    if (metricId is null || !moBroService.TryGet<Metric>(metricId, out _))
    {
      await SendNotFoundAsync(ct);
      return;
    }

    logger.LogDebug("Unregistering metric: {MetricId}", metricId);
    moBroService.Unregister(metricId);

    await SendNoContentAsync(ct);
  }
}