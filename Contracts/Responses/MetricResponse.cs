namespace MoBro.Plugin.RestApi.Contracts.Responses;

public sealed class MetricResponse
{
  public required string Id { get; set; }

  public required string Label { get; set; }

  public required string TypeId { get; set; }

  public required string CategoryId { get; set; }

  public string? Description { get; set; }

  public object? Value { get; set; }

  public DateTime? ValueUpdated { get; set; }
}