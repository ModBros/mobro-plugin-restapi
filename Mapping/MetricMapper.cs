using FastEndpoints;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.SDK.Enums;
using MoBro.Plugin.SDK.Models.Metrics;

namespace MoBro.Plugin.RestApi.Mapping;

public sealed class MetricMapper : Mapper<CreateMetricRequest, MetricResponse, Metric>
{
  public override MetricResponse FromEntity(Metric e)
  {
    return new MetricResponse
    {
      Id = e.Id,
      Label = e.Label,
      Description = e.Description,
      TypeId = e.TypeId,
      CategoryId = e.CategoryId,
      IsStatic = e.IsStatic
    };
  }

  public override Metric ToEntity(CreateMetricRequest r)
  {
    return new Metric(
      r.Id,
      r.Label,
      r.Type?.ToString().ToLower() ?? CoreMetricType.Text.ToString().ToLower(),
      r.Category?.ToString().ToLower() ?? CoreCategory.Miscellaneous.ToString().ToLower()
    )
    {
      Description = r.Description,
      IsStatic = r.IsStatic ?? false,
    };
  }
}