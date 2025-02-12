using Digital.Lib.Net.Core.Messages;

namespace Digital.Lib.Net.Authentication.Models.Events;

public interface IApiEvent
{
    public IApiEvent SetApiUser(Guid apiUserId, Type apiUserType);
    public IApiEvent SetError(Result result);
    public string UserAgent { get; }
    public string IpAddress { get; }
    public Guid? ApiUserId { get; }
    public string? ApiUserType { get; }
    public ApiEventState? State { get; }
    public bool HasError { get; }
    public string? ErrorTrace { get; }
}