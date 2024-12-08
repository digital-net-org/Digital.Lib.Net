using Digital.Net.Entities.Repositories;
using Digital.Net.Entities.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Net.Entities;

public static class Injector
{
    /// <summary>
    ///     Add the Digital Entities services to the service collection.
    ///     This will add the DbContext, IRepository, and IEntityService to the service collection.
    ///     This library is designed to be used with one DbContext per application.
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDigitalEntities<TContext>(this IServiceCollection services)
        where TContext : DbContext =>
        services
            .AddScoped<DbContext, TContext>()
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
}