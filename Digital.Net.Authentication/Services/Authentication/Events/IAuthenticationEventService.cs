using Digital.Net.Authentication.Models;
using Digital.Net.Authentication.Models.Events;
using Digital.Net.Core.Messages;
using Digital.Net.Entities.Models;

namespace Digital.Net.Authentication.Services.Authentication.Events;

public interface IAuthenticationEventService<TApiUser>
    where TApiUser : EntityGuid, IApiUser
{
    public Task RegisterEventAsync(
        AuthenticationEventType eventType,
        ApiEventState eventState,
        Result? result,
        Guid? userId,
        string? payload = null
    );

    public bool HasTooManyAttempts(string payload);
}