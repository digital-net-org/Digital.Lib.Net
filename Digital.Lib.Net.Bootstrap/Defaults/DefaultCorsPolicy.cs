using Digital.Lib.Net.Core.Application.Settings;
using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Bootstrap.Defaults;

public static class DefaultCorsPolicy
{
    /// <summary>
    ///     Allow any method and header for CorsAllowedOrigins configuration content,
    ///     current domain and all it's subdomains.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDefaultCorsPolicy(this WebApplicationBuilder builder)
    {
        var domain = builder.Configuration.GetOrThrow<string>(AppSettings.Domain);
        var allowedOrigins = new List<string>
        {
            $"https://{domain}",
            $"https://*.{domain}",
        };

        allowedOrigins.AddRange(
            builder.Configuration.Get<string[]>(AppSettings.CorsAllowedOrigins)
            ?? []
        );

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(allowedOrigins.ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return builder;
    }
}