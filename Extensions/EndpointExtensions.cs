using System.Text.Json;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace MoBro.Plugin.RestApi.Extensions;

public static class EndpointExtensions
{
  extension(IEndpoint ep)
  {
    public Task SendBadRequest(string detail, CancellationToken ct = default)
    {
      var problemDetailsJson = JsonSerializer.Serialize(new ProblemDetails
      {
        Status = StatusCodes.Status400BadRequest,
        Detail = detail
      });

      ep.HttpContext.Response.Clear();
      ep.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
      ep.HttpContext.Response.ContentType = "application/json";
      return ep.HttpContext.Response.WriteAsync(problemDetailsJson, ct);
    }

    public Task SendConflict(string detail, CancellationToken ct = default)
    {
      var problemDetailsJson = JsonSerializer.Serialize(new ProblemDetails
      {
        Status = StatusCodes.Status409Conflict,
        Detail = detail
      });

      ep.HttpContext.Response.Clear();
      ep.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
      ep.HttpContext.Response.ContentType = "application/json";
      return ep.HttpContext.Response.WriteAsync(problemDetailsJson, ct);
    }
  }
}