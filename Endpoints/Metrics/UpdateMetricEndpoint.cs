using FastEndpoints;
using Microsoft.Extensions.Logging;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.RestApi.Extensions;
using MoBro.Plugin.RestApi.Mapping;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class UpdateMetricEndpoint(IMoBroService moBroService, ILogger logger)
  : Endpoint<CreateMetricRequest, MetricResponse, MetricMapper>
{
  public override void Configure()
  {
    Put("/metrics/{id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateMetricRequest req, CancellationToken ct)
  {
    var metricId = Route<string>("id");
    if (metricId is null || !moBroService.TryGet<Metric>(metricId, out _))
    {
      await SendNotFoundAsync(ct);
      return;
    }

    logger.LogDebug("Updating metric: {MetricId}", req.Id);
    moBroService.Unregister(metricId);
    var entity = Map.ToEntity(req);
    moBroService.Register(entity);
    if (req.Value is not null)
    {
      moBroService.UpdateMetricValue(entity.Id, req.Value.Value.ToObject());
    }

    var response = Map.FromEntity(entity);
    response.Value = moBroService.GetMetricValue(entity.Id)?.Value;
    await SendOkAsync(response, ct);
  }
}