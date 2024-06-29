using System.Text.Json;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace MoBro.Plugin.RestApi.Extensions;

public static class EndpointExtensions
{
  public static Task SendBadRequest(this IEndpoint ep, string detail, CancellationToken ct = default)
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
}