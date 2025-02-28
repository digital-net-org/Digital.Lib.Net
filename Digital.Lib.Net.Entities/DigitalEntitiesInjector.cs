using Digital.Lib.Net.Core.Environment;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.Avatars;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Entities.Seeds;
using Digital.Lib.Net.Entities.Seeds.Development;
using Digital.Lib.Net.Entities.Seeds.Test;
using Digital.Lib.Net.Entities.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Entities;

public static class DigitalEntitiesInjector
{
    /// <summary>
    ///     Add the Digital Entities services to the service collection for the specified type.
    ///     This will add the ISeeder, IRepository, and IEntityService to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="T">Entity to register.</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDigitalEntities<T>(this IServiceCollection services)
        where T : Entity =>
        services
            .AddScoped<IRepository<T, DigitalContext>, Repository<T, DigitalContext>>()
            .AddScoped<IEntityService<T, DigitalContext>, EntityService<T, DigitalContext>>();

    /// <summary>
    ///     Add the DigitalContext and register Entities services for them.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDigitalContext(this IServiceCollection services)
    {
        services.AddScoped<DigitalContext>();
        services.AddDigitalEntities<Avatar>();
        services.AddDigitalEntities<Document>();
        services.AddDigitalEntities<User>();
        services.AddDigitalEntities<ApiToken>();
        services.AddDigitalEntities<ApiKey>();
        services.AddDigitalEntities<Event>();
        return services;
    }

    /// <summary>
    ///     Add data seeds to services.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDataSeeds(this WebApplicationBuilder builder)
    {
        if (AspNetEnv.IsDevelopment)
        {
            builder.Services.AddScoped<ISeed, DevUserSeed>();
        }

        if (AspNetEnv.IsTest)
        {
            builder.Services.AddScoped<ISeed, TestUserSeed>();
        }

        return builder;
    }
}
