using Microsoft.AspNetCore.Http;

namespace Safari.Net.Core.Extensions.HttpUtilities;

public static class Request
{
    /// <summary>
    ///     Get the User-Agent from the HttpRequest.
    /// </summary>
    /// <param name="request">The HttpRequest to get the User-Agent from.</param>
    /// <returns>The User-Agent from the HttpRequest if it exists, otherwise null.</returns>
    public static string? GetUserAgent(this HttpRequest request)
    {
        var result = request.Headers.UserAgent.ToString();
        return string.IsNullOrEmpty(result) ? null : result;
    }

    /// <summary>
    ///     Get the Remote IP Address from the HttpRequest.
    /// </summary>
    /// <param name="request">The HttpRequest to get the Remote IP Address from.</param>
    /// <returns>The Remote IP Address from the HttpRequest if it exists, otherwise null.</returns>
    public static string? GetRemoteIpAddress(this HttpRequest request)
    {
        request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor);
        return
            forwardedFor.FirstOrDefault()
            ?? request.HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    /// <summary>
    ///     Get the Bearer token from the HttpRequest.
    /// </summary>
    /// <param name="request">The HttpRequest to get the Bearer token from.</param>
    /// <returns>The Bearer token from the HttpRequest if it exists, otherwise null.</returns>
    public static string? GetBearerToken(this HttpRequest request) =>
        request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
}