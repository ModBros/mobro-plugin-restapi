using FastEndpoints;
using Microsoft.Extensions.Logging;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.RestApi.Extensions;
using MoBro.Plugin.RestApi.Mapping;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class CreateMetricEndpoint(IMoBroService moBroService, ILogger logger)
  : Endpoint<CreateMetricRequest, MetricResponse, MetricMapper>
{
  public override void Configure()
  {
    Post("/metrics");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateMetricRequest req, CancellationToken ct)
  {
    if (moBroService.TryGet<Metric>(req.Id, out _))
    {
      await this.SendConflict(ct);
      return;
    }

    logger.LogDebug("Creating new metric: {MetricId}", req.Id);
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