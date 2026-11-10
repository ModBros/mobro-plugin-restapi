using System.Text.Json;
using FluentValidation;

namespace MoBro.Plugin.RestApi.Validation;

public static class MetricValueValidation
{
  private const int MaxStringLength = 128;

  public static IRuleBuilderOptions<T, JsonElement?> ValidMetricValue<T>(this IRuleBuilder<T, JsonElement?> ruleBuilder)
  {
    return ruleBuilder
      .Must(jsonElement => jsonElement == null || jsonElement.Value.ValueKind switch
      {
        JsonValueKind.Null => true,
        // numbers beyond the double range (e.g. 1e400) parse to infinity, which cannot be serialized back to JSON
        JsonValueKind.Number => double.IsFinite(jsonElement.Value.GetDouble()),
        JsonValueKind.False or JsonValueKind.True => true,
        JsonValueKind.String => jsonElement.Value.GetString()?.Length <= MaxStringLength,
        // arrays and objects are not supported as metric values
        _ => false
      })
      .WithMessage("Invalid metric value");
  }
}