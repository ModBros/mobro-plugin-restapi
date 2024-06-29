using FastEndpoints;
using Microsoft.Extensions.Logging;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.RestApi.Extensions;
using MoBro.Plugin.RestApi.Mapping;
using MoBro.Plugin.SDK.Exceptions;
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
    var entity = Map.ToEntity(req);

    try
    {
      moBroService.Register(entity);
      logger.LogDebug("Registered new metric: {MetricId}", req.Id);
    }
    catch (MoBroItemValidationException e)
    {
      await this.SendBadRequest(e.Message, ct);
      return;
    }

    if (req.Value is not null)
    {
      try
      {
        moBroService.UpdateMetricValue(entity.Id, req.Value?.ToObject());
      }
      catch (MetricValueValidationException e)
      {
        await this.SendBadRequest(e.Message, ct);
        return;
      }
    }

    var response = Map.FromEntity(entity);

    // set current metric value
    var currValue = moBroService.GetMetricValue(entity.Id);
    response.Value = currValue?.Value;
    response.ValueUpdated = currValue?.Timestamp;

    await SendOkAsync(response, ct);
  }
}