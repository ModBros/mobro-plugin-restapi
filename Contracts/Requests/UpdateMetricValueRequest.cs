using System.Text.Json;

namespace MoBro.Plugin.RestApi.Contracts.Requests;

public sealed class UpdateMetricValueRequest
{
  public JsonElement? Value { get; set; }
}