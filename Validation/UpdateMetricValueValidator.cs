using FastEndpoints;
using MoBro.Plugin.RestApi.Contracts.Requests;

namespace MoBro.Plugin.RestApi.Validation;

public class UpdateMetricValueValidator : Validator<UpdateMetricValueRequest>
{
  public UpdateMetricValueValidator()
  {
    RuleFor(r => r.Value).ValidMetricValue();
  }
}