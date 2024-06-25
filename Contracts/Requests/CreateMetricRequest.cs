using System.Text.Json;

namespace MoBro.Plugin.RestApi.Contracts.Requests;

public sealed class CreateMetricRequest
{
  public required string Id { get; set; }

  public required string Label { get; set; }

  public string? TypeId { get; set; }

  public string? CategoryId { get; set; }

  public bool? IsStatic { get; set; } = false;

  public string? Description { get; set; }

  public string? GroupId { get; set; }

  public JsonElement? Value { get; set; }
}