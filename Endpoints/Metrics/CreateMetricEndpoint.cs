using FastEndpoints;
using Microsoft.Extensions.Logging;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.RestApi.Extensions;
using MoBro.Plugin.RestApi.Mapping;
using MoBro.Plugin.SDK.Enums;
using MoBro.Plugin.SDK.Exceptions;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class CreateMetricEndpoint(
  IMoBroService moBroService,
  ILogger logger
) : Endpoint<CreateMetricRequest, MetricResponse, MetricMapper>
{
  public override void Configure()
  {
    Post("/metrics");
    Version(1);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Register a new metric";
      s.ExampleRequest = new CreateMetricRequest
      {
        Id = "t1",
        Label = "Temperature 1",
        Type = CoreMetricType.Temperature,
        Description = "Some temperature",
      };
      s.ResponseExamples[200] = new MetricResponse
      {
        Id = "ti",
        Label = "Temperature 1",
        TypeId = CoreMetricType.Temperature.ToString(),
        CategoryId = CoreCategory.Miscellaneous.ToString(),
        Description = "Some temperature",
      };
      s.Responses[200] = "The newly registered metric";
      s.Responses[400] = "Validation error";
      s.RequestParam(r => r.Id, "The unique id of the metric");
      s.RequestParam(r => r.Label, "The label for the metric");
      s.RequestParam(r => r.Type, "The type of the metric (optional, default: Text)");
      s.RequestParam(r => r.Category, "The category this metric belongs to (optional, default: Miscellaneous)");
      s.RequestParam(r => r.Description, "A textual description for the metric (optional)");
      s.RequestParam(r => r.Value, "The current value of the metric (optional)");
    });
  }

  public override async Task HandleAsync(CreateMetricRequest req, CancellationToken ct)
  {
    if (moBroService.TryGet<Metric>(req.Id, out _))
    {
      await this.SendConflict("Metric already exists", ct);
      return;
    }

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