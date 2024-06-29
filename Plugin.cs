using System.Text.Json.Serialization;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoBro.Plugin.SDK;
using MoBro.Plugin.SDK.Services;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MoBro.Plugin.RestApi;

public sealed class Plugin : IMoBroPlugin, IDisposable
{
  private readonly ILogger _logger;
  private readonly IMoBroService _mobro;
  private readonly IMoBroSettings _settings;

  private readonly CancellationTokenSource _cancellationTokenSource;

  public Plugin(IMoBroService mobro, IMoBroSettings settings, ILogger logger)
  {
    _logger = logger;
    _mobro = mobro;
    _settings = settings;
    _cancellationTokenSource = new CancellationTokenSource();
  }

  public void Init()
  {
    var port = _settings.GetValue<string>("port");

    var builder = WebApplication.CreateBuilder();

    builder.WebHost.UseUrls($"http://localhost:{port}");
    builder.Services.AddFastEndpoints();
    builder.Services.AddSingleton(_logger);
    builder.Services.AddSingleton(_mobro);
    builder.Services.AddSingleton(_settings);
    builder.Services.ConfigureHttpJsonOptions(options =>
    {
      options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


    var app = builder.Build();

    app.UseDefaultExceptionHandler();
    app.UseFastEndpoints(c =>
    {
      c.Endpoints.RoutePrefix = "api";
      c.Errors.UseProblemDetails();
    });

    // TODO handle error and forward to service
    app.RunAsync(_cancellationTokenSource.Token);
  }

  public void Dispose()
  {
    _cancellationTokenSource.Dispose();
  }
}