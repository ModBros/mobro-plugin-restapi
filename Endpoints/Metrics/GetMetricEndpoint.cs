using FastEndpoints;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.RestApi.Mapping;
using MoBro.Plugin.SDK.Enums;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class GetMetricEndpoint(IMoBroService moBroService) : Endpoint<EmptyRequest, MetricResponse, MetricMapper>
{
  public override void Configure()
  {
    Get("/metrics/{id}");
    Version(1);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Get a specific metric";
      s.ResponseExamples[200] = new MetricResponse
      {
        Id = "ti",
        Label = "Temperature 1",
        TypeId = CoreMetricType.Temperature.ToString(),
        CategoryId = CoreCategory.Miscellaneous.ToString(),
        Description = "Some temperature",
      };
      s.Params["id"] = "The id of the metric";
      s.Responses[200] = "The metric";
      s.Responses[404] = "The metric does not exist";
    });
  }

  public override async Task HandleAsync(EmptyRequest r, CancellationToken ct)
  {
    var id = Route<string>("id");
    if (id is null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (!moBroService.TryGet<Metric>(id, out var metric))
    {
      await SendNotFoundAsync(ct);
      return;
    }

    var response = Map.FromEntity(metric);
    response.Value = moBroService.GetMetricValue(metric.Id)?.Value;
    await SendOkAsync(response, ct);
  }
}