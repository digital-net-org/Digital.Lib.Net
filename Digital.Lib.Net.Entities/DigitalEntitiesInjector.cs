using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.Avatars;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
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
}
