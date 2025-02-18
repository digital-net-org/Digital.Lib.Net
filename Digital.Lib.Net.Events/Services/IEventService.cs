using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models.Events;

namespace Digital.Lib.Net.Events.Services;

public interface IEventService
{
    public Task RegisterEventAsync(
        string name,
        EventState state,
        Result? result,
        Guid? userId,
        string? payload = null
    );
}