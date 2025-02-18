using Digital.Lib.Net.Authentication.Models;

namespace Digital.Lib.Net.Authentication.Services.Authorization;

public interface IAuthorizationJwtService : IAuthorizationService
{
    public AuthorizationResult AuthorizeRefreshToken(string? token);
}