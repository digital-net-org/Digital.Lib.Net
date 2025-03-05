using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models.Users;

namespace Digital.Lib.Net.Entities.Models.Events;

[Table("Event")]
public class Event : EntityId
{
    public Event() {}

    public Event(string name)
    {
        Name = name;
    }

    public Event(string name, string userAgent, string ipAddress)
    {
        Name = name;
        UserAgent = userAgent;
        IpAddress = ipAddress;
    }

    public Event SetUser(Guid? userId)
    {
        UserId = userId ?? UserId;
        return this;
    }

    public Event SetError(Result? result)
    {
        if (result is not null && result.HasError())
        {
            var trace = JsonSerializer.Serialize(result.Errors);
            ErrorTrace = trace.Length > 4096 ? trace[..4096] : trace;
            HasError = true;
        }

        return this;
    }

    [Column("Name"), Required, MaxLength(64)]
    public string Name { get; private set; } = string.Empty;

    [Column("Payload"), MaxLength(64)]
    public string? Payload { get; set; }

    [Column("UserAgent"), Required, MaxLength(1024)]
    public string UserAgent { get; set; } = string.Empty;

    [Column("IpAddress"), Required, MaxLength(45)]
    public string IpAddress { get; set; } = string.Empty;

    [Column("UserId"), ForeignKey("User")]
    public Guid? UserId { get; private set; }

    public virtual User? User { get; set; }

    [Column("State")]
    public EventState? State { get; set; }

    [Column("HasError"), Required]
    public bool HasError { get; private set; }

    [Column("ErrorTrace"), MaxLength(4096)]
    public string? ErrorTrace { get; private set; }
}
