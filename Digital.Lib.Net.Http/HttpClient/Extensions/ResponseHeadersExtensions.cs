using System.Net.Http.Headers;

namespace Digital.Lib.Net.Http.HttpClient.Extensions;

public static class ResponseHeadersExtensions
{
    public static string? TryGetCookie(this HttpResponseHeaders headers, string cookieName)
    {
        headers.TryGetValues("Set-Cookie", out var values);
        var result = values?.FirstOrDefault(value => value.Contains(cookieName))?.Split(';')[0];
        return result?.Split($"{cookieName}=")[1];
    }
}