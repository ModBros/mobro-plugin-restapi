using System.Text.Json;

namespace MoBro.Plugin.RestApi.Extensions;

public static class JsonElementExtension
{
  public static object? ToObject(this JsonElement jsonElement) => jsonElement.ValueKind switch
  {
    JsonValueKind.Null => null,
    JsonValueKind.False => false,
    JsonValueKind.True => true,
    JsonValueKind.Number => jsonElement.GetDouble(),
    JsonValueKind.String => jsonElement.GetString(),
    _ => null
  };
}