namespace MoBro.Plugin.RestApi.Contracts.Responses;

public sealed class MetricResponse
{
  public required string Id { get; set; }

  public required string Label { get; set; }

  public required string TypeId { get; set; }

  public required string CategoryId { get; set; }

  public bool IsStatic { get; set; }

  public string? Description { get; set; }

  public string? GroupId { get; set; }

  public object? Value { get; set; }
}