using System.Text.Json;
using FastEndpoints;
using FluentValidation;
using MoBro.Plugin.RestApi.Contracts.Requests;
using MoBro.Plugin.SDK.Models.Metrics;
using MoBro.Plugin.SDK.Services;

namespace MoBro.Plugin.RestApi.Validation;

public class MetricValidator : Validator<CreateMetricRequest>
{
  public MetricValidator(IMoBroService service)
  {
    RuleFor(r => r.Id)
      .Cascade(CascadeMode.Stop)
      .NotNull()
      .NotEmpty()
      .Length(1, 128)
      .Matches(@"^[\w\.\-]+$")
      .Must(id => !service.TryGet<Metric>(id, out _))
      .WithMessage("Metric already exists by this id");

    RuleFor(r => r.Label)
      .NotNull()
      .NotEmpty()
      .Length(1, 64);

    RuleFor(r => r.Description)
      .MaximumLength(256);

    RuleFor(r => r.Value)
      .Must(jsonElement => jsonElement != null && jsonElement.Value.ValueKind switch
      {
        JsonValueKind.Number => true,
        JsonValueKind.False or JsonValueKind.True => true,
        JsonValueKind.String => jsonElement.Value.GetString()?.Length < 128,
        _ => false
      })
      .When(r => r.Value != null)
      .WithMessage("Invalid metric value");
  }
}