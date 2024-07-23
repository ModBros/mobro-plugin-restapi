using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoBro.Plugin.SDK;
using MoBro.Plugin.SDK.Services;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MoBro.Plugin.RestApi;

public sealed class Plugin(IMoBroService service, IMoBroSettings settings, ILogger logger) : IMoBroPlugin, IDisposable
{
  private readonly CancellationTokenSource _cancellationTokenSource = new();

  private Task? _appRunTask;

  public void Init()
  {
    var port = settings.GetValue("port", 8080);
    var swagger = settings.GetValue("swagger_enable", false);

    var app = BuildWebApplication(port, swagger);

    logger.LogInformation("Starting on port {Port}", port);
    _appRunTask = app.RunAsync(_cancellationTokenSource.Token).ContinueWith(t =>
    {
      if (t.IsFaulted)
      {
        service.Error(t.Exception);
      }
    });
  }

  private WebApplication BuildWebApplication(int port, bool swagger)
  {
    var bld = WebApplication.CreateBuilder();

    bld.WebHost.UseUrls($"http://localhost:{port}");
    bld.Services
      .AddFastEndpoints()
      .AddSingleton(logger)
      .AddSingleton(service)
      .AddSingleton(settings)
      .ConfigureHttpJsonOptions(options =>
      {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
      });

    if (swagger)
    {
      bld.Services.SwaggerDocument(o =>
      {
        o.ShortSchemaNames = true;
        o.MaxEndpointVersion = 1;
        o.DocumentSettings = s =>
        {
          s.DocumentName = "MoBro REST API v1";
          s.Version = "v1";
        };
      });
    }

    var app = bld.Build();
    app
      .UseDefaultExceptionHandler()
      .UseFastEndpoints(c =>
      {
        c.Endpoints.RoutePrefix = "api";
        c.Errors.UseProblemDetails();
        c.Endpoints.ShortNames = true;
        c.Versioning.Prefix = "v";
        c.Versioning.PrependToRoute = true;
      });

    if (swagger)
    {
      app.UseSwaggerGen();
    }

    return app;
  }

  public void Dispose()
  {
    _cancellationTokenSource.Cancel();
    _appRunTask?.Wait(TimeSpan.FromSeconds(10));
    _cancellationTokenSource.Dispose();
  }
}