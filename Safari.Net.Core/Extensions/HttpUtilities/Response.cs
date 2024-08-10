using Microsoft.AspNetCore.Http;

namespace Safari.Net.Core.Extensions.HttpUtilities;

public static class Response
{
    /// <summary>
    ///     Set a cookie on the HttpResponse.
    /// </summary>
    /// <param name="response">The HttpResponse to set the cookie on.</param>
    /// <param name="content">The content of the cookie.</param>
    /// <param name="name">The name of the cookie.</param>
    /// <param name="expiration">The expiration of the cookie.</param>
    /// <param name="sameSite">The SameSiteMode of the cookie.</param>
    /// <param name="httpOnly">The HttpOnly of the cookie.</param>
    /// <param name="secure">The Secure of the cookie.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SetCookie(
        this HttpResponse response,
        string content,
        string name,
        long expiration,
        SameSiteMode? sameSite = null,
        bool? httpOnly = null,
        bool? secure = null
    )
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = httpOnly ?? true,
            Secure = secure ?? true,
            SameSite = sameSite ?? SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMilliseconds(expiration)
        };
        response.Cookies.Append(name, content, cookieOptions);
    }
}