using Digital.Net.Entities.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Net.Entities;

public static class Injector
{
    public static IServiceCollection AddDigitalEntities(this IServiceCollection services) =>
        services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
}