using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Models.Events;
using Digital.Lib.Net.Authentication.Services.Options;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Services;

namespace Digital.Lib.Net.Authentication.Services.Authentication.Events;

public class AuthenticationEventService<TApiUser, TEvent>(
    IHttpContextService httpContextService,
    IJwtOptionService jwtOptionService,
    IRepository<TEvent> repository
) : IAuthenticationEventService<TApiUser>
    where TApiUser : EntityGuid, IApiUser
    where TEvent : AuthenticationEvent, new()
{
    public async Task RegisterEventAsync(
        AuthenticationEventType eventType,
        ApiEventState eventState,
        Result? result,
        Guid? userId,
        string? payload = null
    )
    {
        var authEvent = AuthenticationEvent.Create<TEvent>(
            eventType,
            eventState,
            httpContextService.UserAgent,
            httpContextService.IpAddress,
            payload
        );
        authEvent.SetError(result);

        if (userId.HasValue)
            authEvent.SetApiUser(userId.Value, typeof(TApiUser));

        await repository.CreateAsync(authEvent);
        await repository.SaveAsync();
    }

    public bool HasTooManyAttempts(string payload)
    {
        var threshold = DateTime.UtcNow.Subtract(jwtOptionService.GetLoginAttemptThreshold());
        var count = repository.Count(e =>
            e.CreatedAt > threshold
            && e.EventType == AuthenticationEventType.Login
            && e.State == ApiEventState.Failed
            && e.Payload == payload
            && e.IpAddress == httpContextService.IpAddress
        );
        return count >= jwtOptionService.MaxLoginAttempts;
    }
}