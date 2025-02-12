using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Models.Authorizations;
using Digital.Lib.Net.Entities.Models;

namespace Digital.Lib.Net.Authentication.Services.Authorization;

public interface IAuthorizationJwtService<TApiUser> : IAuthorizationService
    where TApiUser : EntityGuid, IApiUser
{
    public AuthorizationResult AuthorizeApiUserRefresh(string? token);
}