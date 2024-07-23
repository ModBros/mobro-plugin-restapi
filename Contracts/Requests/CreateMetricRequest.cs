using System.ComponentModel;
using System.Text.Json;
using MoBro.Plugin.SDK.Enums;

namespace MoBro.Plugin.RestApi.Contracts.Requests;

public sealed class CreateMetricRequest
{
  public required string Id { get; set; }

  public required string Label { get; set; }

  [DefaultValue(CoreMetricType.Text)] 
  public CoreMetricType? Type { get; set; }

  [DefaultValue(CoreCategory.Miscellaneous)]
  public CoreCategory? Category { get; set; }

  [DefaultValue(false)] 
  public bool? IsStatic { get; set; } = false;

  public string? Description { get; set; }

  public JsonElement? Value { get; set; }
}