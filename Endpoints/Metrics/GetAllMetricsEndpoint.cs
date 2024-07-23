using FastEndpoints;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.RestApi.Mapping;
using MoBro.Plugin.SDK.Enums;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class GetAllMetricsEndpoint(IMoBroService moBroService)
  : Endpoint<EmptyRequest, IEnumerable<MetricResponse>, MetricMapper>
{
  public override void Configure()
  {
    Get("/metrics");
    Version(1);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Get all registered metrics";
      s.ResponseExamples[200] = new[]
      {
        new MetricResponse
        {
          Id = "ti",
          Label = "Temperature 1",
          TypeId = CoreMetricType.Temperature.ToString(),
          CategoryId = CoreCategory.Miscellaneous.ToString(),
          Description = "Some temperature",
          IsStatic = false
        }
      };
      s.Responses[200] = "All registered metrics";
    });
  }

  public override Task<IEnumerable<MetricResponse>> ExecuteAsync(EmptyRequest r, CancellationToken ct)
  {
    return Task.FromResult(
      moBroService.GetAll<Metric>()
        .Select(m => Map.FromEntity(m))
        .Select(m =>
        {
          // set current metric value
          var currValue = moBroService.GetMetricValue(m.Id);
          m.Value = currValue?.Value;
          m.ValueUpdated = currValue?.Timestamp;
          return m;
        })
    );
  }
}