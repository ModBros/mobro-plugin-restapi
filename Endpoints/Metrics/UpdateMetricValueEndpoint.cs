using FastEndpoints;
using Microsoft.Extensions.Logging;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.RestApi.Extensions;
using MoBro.Plugin.SDK.Exceptions;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class UpdateMetricValueEndpoint(IMoBroService moBroService, ILogger logger)
  : Endpoint<UpdateMetricValueRequest>
{
  public override void Configure()
  {
    Put("/metrics/{id}/value");
    Version(1);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Update the value of a metric";
      s.ExampleRequest = new UpdateMetricValueRequest
      {
        Value = null
      };
      s.Params["id"] = "The id of the metric";
      s.Responses[204] = "The value of the metric was successfully updated";
      s.Responses[400] = "Invalid metric value";
      s.Responses[404] = "The metric does not exist";
      s.RequestParam(r => r.Value, "The new value of the metric");
    });
  }

  public override async Task HandleAsync(UpdateMetricValueRequest req, CancellationToken ct)
  {
    var metricId = Route<string>("id");
    if (metricId is null || !moBroService.TryGet<Metric>(metricId, out _))
    {
      await SendNotFoundAsync(ct);
      return;
    }

    try
    {
      moBroService.UpdateMetricValue(metricId, req.Value?.ToObject());
      logger.LogDebug("Updated value of metric: {MetricId}", metricId);
    }
    catch (MetricValueValidationException e)
    {
      await this.SendBadRequest(e.Message, ct);
      return;
    }

    await SendNoContentAsync(ct);
  }
}