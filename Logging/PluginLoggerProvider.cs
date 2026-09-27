using Microsoft.Extensions.Logging;

namespace MoBro.Plugin.RestApi.Logging;

/// <summary>
/// Routes all ASP.NET Core logging into the logger provided by the plugin SDK.
/// </summary>
public sealed class PluginLoggerProvider(ILogger logger) : ILoggerProvider
{
  public ILogger CreateLogger(string categoryName) => logger;

  public void Dispose()
  {
  }
}