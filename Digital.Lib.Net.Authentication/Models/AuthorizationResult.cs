using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models.Users;

namespace Digital.Lib.Net.Authentication.Models;

public class AuthorizationResult : Result
{
    public Guid UserId { get; set; }
    public UserRole Role { get; set; }

    public bool IsForbidden { get; private set; }
    public void Forbid() => IsForbidden = true;
    public bool IsAuthorized { get; private set; }
    public void Authorize() => IsAuthorized = true;

    public new AuthorizationResult Merge(Result result)
    {
        base.Merge(result);
        if (result is AuthorizationResult authResult)
        {
            IsAuthorized = authResult.IsAuthorized;
            UserId = authResult.UserId;
        }
        return this;
    }

    public AuthorizationResult AddError(Exception e)
    {
        base.AddError(e);
        return this;
    }
}