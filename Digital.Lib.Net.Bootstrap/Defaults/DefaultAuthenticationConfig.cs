using System.Text.RegularExpressions;
using Digital.Lib.Net.Authentication;
using Digital.Lib.Net.Authentication.Options.Config;
using Digital.Lib.Net.Core.Application.Settings;
using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Digital.Lib.Net.Core.Extensions.StringUtilities;
using Microsoft.AspNetCore.Builder;

namespace Digital.Lib.Net.Bootstrap.Defaults;

public static class DefaultAuthenticationConfig
{
    /// <summary>
    ///     Add Jwt/Api-key strategy to the application.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        var domain = builder.Configuration.GetOrThrow<string>(AppSettings.Domain);
        var pswRegex = builder.Configuration.Get<string>(AppSettings.AuthPasswordRegex);

        builder.Services.AddDigitalAuthentication(opts =>
        {
            opts.SetJwtTokenOptions(new JwtTokenConfig
            {
                Secret = builder.Configuration.GetOrThrow<string>(AppSettings.AuthJwtSecret),
                Issuer = $"https://{domain}",
                Audience = $"https://{domain}",
                RefreshTokenExpiration = builder.Configuration.Get<long>(AppSettings.AuthJwtRefreshExpiration),
                AccessTokenExpiration = builder.Configuration.Get<long>(AppSettings.AuthJwtBearerExpiration),
                CookieName = $"{domain}_refresh",
            });
            opts.SetApiKeyOptions(new ApiKeyConfig
            {
                HeaderAccessor = $"{domain}_auth"
            });
            opts.SetPasswordOptions(new PasswordConfig
            {
                PasswordRegex =  pswRegex is not null
                    ? new Regex(pswRegex)
                    : RegularExpressions.Password,
            });
        });
        return builder;
    }
}