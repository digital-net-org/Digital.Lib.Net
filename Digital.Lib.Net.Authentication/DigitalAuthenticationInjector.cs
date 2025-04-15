using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Authentication.Options.Config;
using Digital.Lib.Net.Authentication.Services.Authentication;
using Digital.Lib.Net.Authentication.Services.Authorization;
using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Digital.Lib.Net.Events.Services;
using Digital.Lib.Net.Sdk.Services.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Digital.Lib.Net.Authentication;

public static class DigitalAuthenticationInjector
{
    /// <summary>
    ///     Add Jwt/Api-key strategy to the application.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDigitalAuthentication(this WebApplicationBuilder builder)
    {
        var domain = builder.Configuration.GetOrThrow<string>(AppSettings.Domain);
        builder.Services.Configure<AuthenticationOptions>(opts =>
        {
            opts.SetJwtTokenOptions(new JwtTokenConfig
            {
                Issuer = $"https://{domain}",
                Audience = $"https://{domain}",
                CookieName = $"{domain}_refresh",
            });
            opts.SetApiKeyOptions(new ApiKeyConfig
            {
                HeaderAccessor = $"{domain}_auth"
            });
        });
        builder.Services.TryAddScoped<IEventService, EventService>();
        builder.Services
            .AddScoped<IUserContextService, UserContextService>()
            .AddScoped<IAuthenticationOptionService, AuthenticationOptionService>()
            .AddScoped<IAuthorizationJwtService, AuthorizationJwtService>()
            .AddScoped<IAuthorizationApiKeyService, AuthorizationApiKeyService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IAuthenticationJwtService, AuthenticationJwtService>();
        return builder;
    }
}