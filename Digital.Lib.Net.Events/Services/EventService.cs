using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Services;

namespace Digital.Lib.Net.Events.Services;

public class EventService(
    IHttpContextService httpContextService,
    IRepository<Event, DigitalContext> eventRepository
) : IEventService
{
    public async Task RegisterEventAsync(
        string name,
        EventState state,
        Result? result,
        Guid? userId,
        string? payload = null
    )
    {
        await eventRepository.CreateAsync(
            new Event(name, httpContextService.UserAgent, httpContextService.IpAddress)
                .SetUser(userId)
                .SetError(result)
        );
        await eventRepository.SaveAsync();
    }
}