using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Models.Events;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models;

namespace Digital.Lib.Net.Authentication.Services.Authentication.Events;

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