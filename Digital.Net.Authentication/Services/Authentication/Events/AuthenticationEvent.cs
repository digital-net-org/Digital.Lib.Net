using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digital.Net.Authentication.Models.Events;
using Digital.Net.Core.Extensions.EnumUtilities;

namespace Digital.Net.Authentication.Services.Authentication.Events;

public abstract class AuthenticationEvent : ApiEvent
{
    protected AuthenticationEvent()
    {
    }

    protected AuthenticationEvent(
        AuthenticationEventType eventType,
        ApiEventState eventState,
        string userAgent,
        string ipAddress,
        string? payload = null
    )
    {
        Initialize(eventType, eventState, userAgent, ipAddress, payload);
    }

    [Column("Action"), Required]
    public AuthenticationEventType EventType { get; private set; } = AuthenticationEventType.Unknown;

    [Column("ActionName"), Required, MaxLength(128)]
    public string ActionName { get; private set; } = string.Empty;

    [Column("Payload")]
    public string? Payload { get; private set; } = string.Empty;

    public static TEvent Create<TEvent>(
        AuthenticationEventType eventType,
        ApiEventState eventState,
        string userAgent,
        string ipAddress,
        string? payload = null
    ) where TEvent : AuthenticationEvent, new()
    {
        var eventInstance = new TEvent();
        eventInstance.Initialize(eventType, eventState, userAgent, ipAddress, payload);
        return eventInstance;
    }

    protected void Initialize(
        AuthenticationEventType eventType,
        ApiEventState eventState,
        string userAgent,
        string ipAddress,
        string? payload
    )
    {
        EventType = eventType;
        State = eventState;
        ActionName = eventType.GetDisplayName();
        UserAgent = userAgent;
        IpAddress = ipAddress;
        Payload = payload;
    }
}
