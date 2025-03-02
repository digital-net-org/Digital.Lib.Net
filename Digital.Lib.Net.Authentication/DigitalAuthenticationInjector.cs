using System.Text.RegularExpressions;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Authentication.Options.Config;
using Digital.Lib.Net.Authentication.Services.Authentication;
using Digital.Lib.Net.Authentication.Services.Authorization;
using Digital.Lib.Net.Core.Application;
using Digital.Lib.Net.Core.Application.Settings;
using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Digital.Lib.Net.Core.Extensions.StringUtilities;
using Digital.Lib.Net.Events.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Digital.Lib.Net.Authentication;

public static class DigitalAuthenticationInjector
{
    /// <summary>
    ///     Adds the Authentication services to the service collection. Use the <see cref="AuthenticationOptions" />
    ///     object to configure the options.
    /// </summary>
    /// <param name="services"> The service collection to add the service to. </param>
    /// <param name="buildOptions"> Object config. </param>
    public static IServiceCollection AddDigitalAuthentication(
        this IServiceCollection services,
        Action<AuthenticationOptions> buildOptions
    )
    {
        services.ConfigureOptions(buildOptions);
        services.TryAddScoped<IEventService, EventService>();

        services
            .AddScoped<IAuthenticationOptionService, AuthenticationOptionService>()
            .AddScoped<IAuthorizationJwtService, AuthorizationJwtService>()
            .AddScoped<IAuthorizationApiKeyService, AuthorizationApiKeyService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IAuthenticationJwtService, AuthenticationJwtService>();
        return services;
    }

    /// <summary>
    ///     Add Jwt/Api-key strategy to the application.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDigitalAuthentication(this WebApplicationBuilder builder)
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