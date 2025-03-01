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
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDigitalContext(this WebApplicationBuilder builder)
    {
        builder.AddNpgsqlContext<DigitalContext>();
        builder.Services
            .AddDigitalEntities<Avatar>()
            .AddDigitalEntities<Document>()
            .AddDigitalEntities<User>()
            .AddDigitalEntities<ApiToken>()
            .AddDigitalEntities<ApiKey>()
            .AddDigitalEntities<Event>();
        return builder;
    }

    /// <summary>
    ///     Add data seeds to services.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDataSeeds(this IServiceCollection services)
    {
        if (AspNetEnv.IsDevelopment)
        {
            services.AddScoped<ISeed, DevUserSeed>();
        }
        if (AspNetEnv.IsTest)
        {
            services.AddScoped<ISeed, TestUserSeed>();
        }
        return services;
    }
}
