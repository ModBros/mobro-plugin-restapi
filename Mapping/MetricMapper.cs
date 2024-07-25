using FastEndpoints;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.RestApi.Contracts.Responses;
using MoBro.Plugin.SDK.Enums;
using MoBro.Plugin.SDK.Models.Metrics;

namespace MoBro.Plugin.RestApi.Mapping;

public sealed class MetricMapper : Mapper<CreateMetricRequest, MetricResponse, Metric>
{
  public override MetricResponse FromEntity(Metric e) => new()
  {
    Id = e.Id,
    Label = e.Label,
    Description = e.Description,
    TypeId = e.TypeId,
    CategoryId = e.CategoryId,
  };

  public override Metric ToEntity(CreateMetricRequest r) => new()
  {
    Id = r.Id,
    Label = r.Label,
    TypeId = r.Type?.ToString().ToLower() ?? CoreMetricType.Text.ToString().ToLower(),
    CategoryId = r.Category?.ToString().ToLower() ?? CoreCategory.Miscellaneous.ToString().ToLower(),
    Description = r.Description,
    IsStatic = false,
  };
}