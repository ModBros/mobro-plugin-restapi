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
    RuleFor(r => r)
      .Must(_ => service.GetAll<Metric>().Count() < 100)
      .WithMessage("Maximum number of metrics reached");

    RuleFor(r => r.Id)
      .Cascade(CascadeMode.Stop)
      .NotNull()
      .NotEmpty()
      .Length(1, 128)
      .Matches(@"^[\w\.\-]+$");

    RuleFor(r => r.Label)
      .NotNull()
      .NotEmpty()
      .Length(1, 64);

    RuleFor(r => r.Description)
      .MaximumLength(256);

    RuleFor(r => r.Value)
      .ValidMetricValue();
  }
}