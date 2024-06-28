using System.Text.Json;
using MoBro.Plugin.SDK.Enums;

namespace MoBro.Plugin.RestApi.Contracts.Requests;

public sealed class CreateMetricRequest
{
  public required string Id { get; set; }

  public required string Label { get; set; }

  public CoreMetricType? Type { get; set; }

  public CoreCategory? Category { get; set; }

  public bool? IsStatic { get; set; } = false;

  public string? Description { get; set; }

  public JsonElement? Value { get; set; }
}