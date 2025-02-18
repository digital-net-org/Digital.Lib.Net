using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Authentication.Services.Authentication;
using Digital.Lib.Net.Authentication.Services.Authorization;
using Digital.Lib.Net.Core.Application;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IAuthenticationOptionService, AuthenticationOptionService>();
        services.AddScoped<IAuthorizationJwtService, AuthorizationJwtService>();
        services.AddScoped<IAuthorizationApiKeyService, AuthorizationApiKeyService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}