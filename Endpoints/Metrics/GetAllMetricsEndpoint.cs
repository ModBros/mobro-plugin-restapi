using FastEndpoints;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.RestApi.Mapping;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Endpoints.Metrics;

public sealed class GetAllMetricsEndpoint(IMoBroService moBroService)
  : Endpoint<EmptyRequest, IEnumerable<MetricResponse>, MetricMapper>
{
  public override void Configure()
  {
    Get("/metrics");
    AllowAnonymous();
  }

  public override Task<IEnumerable<MetricResponse>> ExecuteAsync(EmptyRequest r, CancellationToken ct)
  {
    return Task.FromResult(
      moBroService.GetAll<Metric>()
        .Select(m => Map.FromEntity(m))
        .Select(m =>
        {
          m.Value = moBroService.GetMetricValue(m.Id)?.Value;
          return m;
        })
    );
  }
}