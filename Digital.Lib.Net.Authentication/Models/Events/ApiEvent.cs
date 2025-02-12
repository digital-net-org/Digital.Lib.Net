using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models;

namespace Digital.Lib.Net.Authentication.Models.Events;

public abstract class ApiEvent : EntityId, IApiEvent
{
    public ApiEvent()
    {
    }

    public ApiEvent(string userAgent, string ipAddress)
    {
        UserAgent = userAgent;
        IpAddress = ipAddress;
    }

    public IApiEvent SetApiUser(Guid apiUserId, Type apiUserType)
    {
        ApiUserId = apiUserId;
        ApiUserType = apiUserType.Name;
        return this;
    }

    public IApiEvent SetError(Result? result)
    {
        if (result is not null && result.HasError())
        {
            var trace = JsonSerializer.Serialize(result.Errors);
            ErrorTrace = trace.Length > 4096 ? trace[..4096] : trace;
            HasError = true;
        }

        return this;
    }

    [Column("UserAgent"), Required, MaxLength(1024)]
    public string UserAgent { get; set; } = string.Empty;

    [Column("IpAddress"), Required, MaxLength(45)]
    public string IpAddress { get; set; } = string.Empty;

    [Column("ApiUserId")]
    public Guid? ApiUserId { get; private set; }

    [Column("ApiUserType"), MaxLength(128)]
    public string? ApiUserType { get; private set; }

    [Column("State")]
    public ApiEventState? State { get; set; }

    [Column("HasError"), Required]
    public bool HasError { get; private set; }

    [Column("ErrorTrace"), MaxLength(4096)]
    public string? ErrorTrace { get; private set; }
}