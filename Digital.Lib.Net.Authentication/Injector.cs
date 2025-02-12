using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Models.Authorizations;
using Digital.Lib.Net.Authentication.Options.ApiKey;
using Digital.Lib.Net.Authentication.Options.Jwt;
using Digital.Lib.Net.Authentication.Services.Authentication;
using Digital.Lib.Net.Authentication.Services.Authentication.ApiUsers;
using Digital.Lib.Net.Authentication.Services.Authentication.Events;
using Digital.Lib.Net.Authentication.Services.Authorization;
using Digital.Lib.Net.Authentication.Services.Options;
using Digital.Lib.Net.Authentication.Services.Security;
using Digital.Lib.Net.Core.Application;
using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc;
using Digital.Lib.Net.Mvc.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Authentication;

public static class Injector
{
    /// <summary>
    ///     Configures the JWT options for the application. Needed for the JWT authentication service.
    /// </summary>
    /// <param name="services"> The service collection to add the service to. </param>
    /// <param name="buildOptions"> The action to build the options. </param>
    /// <returns></returns>
    public static IServiceCollection AddDigitalJwtOptions(
        this IServiceCollection services,
        Action<JwtAuthenticationOptions> buildOptions
    )
    {
        services.ConfigureOptions(buildOptions);
        services.AddScoped<IJwtOptionService, JwtOptionService>();
        return services;
    }

    /// <summary>
    ///     Adds the JWT authentication service to the service collection. Use the <see cref="AddDigitalJwtOptions" />
    ///     method to configure the options.
    /// </summary>
    /// <param name="services"> The service collection to add the service to. </param>
    /// <typeparam name="TApiUser"> The EFCore model to be used for the API User. </typeparam>
    /// <typeparam name="TApiToken"> The EFCore model to be used for the API Token. </typeparam>
    /// <typeparam name="TAuthEvent"> The EFCore model to be used for the Authentication Event logs. </typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDigitalJwtAuthentication<TApiUser, TApiToken, TAuthEvent>(
        this IServiceCollection services
    )
        where TApiUser : EntityGuid, IApiUser
        where TApiToken : AuthorizationToken, new()
        where TAuthEvent : AuthenticationEvent, new()
    {
        if (!services.IsInjected(typeof(IHttpContextService)))
            services.AddDigitalMvc();
        if (!services.IsInjected(typeof(IRepository<TApiUser>)))
            services.AddScoped<IRepository<TApiUser>, Repository<TApiUser>>();
        if (!services.IsInjected(typeof(IRepository<TApiToken>)))
            services.AddScoped<IRepository<TApiToken>, Repository<TApiToken>>();
        if (!services.IsInjected(typeof(IRepository<TAuthEvent>)))
            services.AddScoped<IRepository<TAuthEvent>, Repository<TAuthEvent>>();
        if (!services.IsInjected(typeof(IHashService)))
            services.AddScoped<IHashService, HashService>();
        if (!services.IsInjected(typeof(IAuthenticationJwtService)))
            services.AddScoped<IAuthenticationJwtService, AuthenticationJwtService<TApiToken>>();

        services.AddScoped<IApiUserService<TApiUser>, ApiUserService<TApiUser>>();
        services.AddScoped<IAuthenticationEventService<TApiUser>, AuthenticationEventService<TApiUser, TAuthEvent>>();
        services.AddScoped<IAuthorizationJwtService<TApiUser>, AuthorizationJwtService<TApiUser, TApiToken>>();
        services.AddScoped<IAuthenticationService<TApiUser>, AuthenticationService<TApiUser, TApiToken>>();
        return services;
    }

    /// <summary>
    ///     Adds the API Key authentication service to the service collection. Use the
    ///     <see cref="ApiKeyAuthenticationOptions" /> to configure the options.
    /// </summary>
    /// <param name="services"> The service collection to add the service to. </param>
    /// <param name="buildOptions"> The action to build the options. </param>
    /// <typeparam name="TApiKey">
    ///     The EFCore model to be used for the API Key. The Key should have a user ID reference with
    ///     "TApiUser" parameter.
    /// </typeparam>
    /// <typeparam name="TApiUser"> The EFCore model to be used for the API User. </typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDigitalApiKeyAuthentication<TApiUser, TApiKey>(
        this IServiceCollection services,
        Action<ApiKeyAuthenticationOptions>? buildOptions = null
    )
        where TApiUser : EntityGuid, IApiUser
        where TApiKey : AuthorizationApiKey, new()
    {
        services.ConfigureOptions(buildOptions);
        services.AddScoped<IAuthorizationApiKeyService<TApiUser>, AuthorizationApiKeyService<TApiUser, TApiKey>>();

        if (!services.IsInjected(typeof(IHttpContextService)))
            services.AddDigitalMvc();
        if (!services.IsInjected(typeof(IRepository<TApiKey>)))
            services.AddScoped<IRepository<TApiKey>, Repository<TApiKey>>();

        return services;
    }
}