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
        var appEvent = new Event
        {
            Name = name,
            State = state,
            UserId = userId,
            Payload = payload,
            UserAgent = httpContextService.UserAgent, 
            IpAddress = httpContextService.IpAddress
        };
        
        if (result is not null && result.HasError())
            appEvent.SetError(result);
        
        await eventRepository.CreateAndSaveAsync(appEvent);
    }
}