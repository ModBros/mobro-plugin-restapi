using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace MoBro.Plugin.RestApi.Extensions;

public static class EndpointExtensions
{
  public static Task SendConflict(this IEndpoint ep, CancellationToken ct = default)
  {
    ep.HttpContext.MarkResponseStart();
    ep.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
    return ep.HttpContext.Response.StartAsync(ct);
  }
}