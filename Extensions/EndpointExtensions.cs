using System.Text.Json;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace MoBro.Plugin.RestApi.Extensions;

public static class EndpointExtensions
{
  extension(IEndpoint ep)
  {
    public Task SendBadRequest(string detail, CancellationToken ct = default) =>
      SendProblem(ep, StatusCodes.Status400BadRequest, detail, ct);

    public Task SendConflict(string detail, CancellationToken ct = default) =>
      SendProblem(ep, StatusCodes.Status409Conflict, detail, ct);
  }

  private static Task SendProblem(IEndpoint ep, int status, string detail, CancellationToken ct)
  {
    var problemDetailsJson = JsonSerializer.Serialize(new ProblemDetails
    {
      Status = status,
      Detail = detail
    }, JsonSerializerOptions.Web);

    ep.HttpContext.Response.Clear();
    ep.HttpContext.Response.StatusCode = status;
    ep.HttpContext.Response.ContentType = "application/json";
    return ep.HttpContext.Response.WriteAsync(problemDetailsJson, ct);
  }
}