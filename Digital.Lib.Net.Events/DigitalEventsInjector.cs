using Digital.Lib.Net.Events.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Events;

public static class DigitalEventsInjector
{
    public static IServiceCollection AddDigitalEventServices(this IServiceCollection services) =>
        services.AddScoped<IEventService, EventService>();
}