using Safari.Net.Core.Extensions.HttpUtilities;

namespace Safari.Net.Core.Tests.Extensions.HttpUtilities;

public class HttpHeadersTests
{
    [Fact]
    public void TryGetHeaderValue_ReturnsValue_WhenHeaderExists()
    {
        var response = new HttpResponseMessage();
        response.Headers.Add("Test-Header", "TestValue");
        var result = response.TryGetHeaderValue("Test-Header");
        Assert.Equal("TestValue", result);
    }

    [Fact]
    public void TryGetHeaderValue_ReturnsNull_WhenHeaderDoesNotExist()
    {
        var response = new HttpResponseMessage();
        var result = response.TryGetHeaderValue("Non-Existent-Header");
        Assert.Null(result);
    }

    [Fact]
    public void TryGetCookie_ReturnsCookieValue_WhenSetCookieHeaderExists()
    {
        var response = new HttpResponseMessage();
        response.Headers.Add("Set-Cookie", "cookieValue");
        var result = response.TryGetCookie();
        Assert.Equal("cookieValue", result);
    }

    [Fact]
    public void TryGetCookie_ReturnsNull_WhenSetCookieHeaderDoesNotExist()
    {
        var response = new HttpResponseMessage();
        var result = response.TryGetCookie();
        Assert.Null(result);
    }

    [Fact]
    public void AddCookie_AddsCookieToClientHeaders()
    {
        const string cookie = "testCookie";
        var client = new HttpClient();
        client.AddCookie(cookie);
        Assert.True(client.DefaultRequestHeaders.Contains(HttpHeaders.CookieHeader));
        Assert.Equal(cookie, client.DefaultRequestHeaders.GetValues(HttpHeaders.CookieHeader).First());
    }

    [Fact]
    public void AddAuthorization_AddsBearerTokenToClientHeaders()
    {
        var client = new HttpClient();
        const string token = "testToken";
        client.AddAuthorization(token);
        Assert.NotNull(client.DefaultRequestHeaders.Authorization);
        Assert.Equal(HttpHeaders.BearerAuthorization, client.DefaultRequestHeaders.Authorization.Scheme);
        Assert.Equal(token, client.DefaultRequestHeaders.Authorization.Parameter);
    }
}