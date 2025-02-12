using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Models.Authorizations;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Services;

namespace Digital.Lib.Net.Authentication.Services.Authentication.ApiUsers;

public class ApiUserService<TApiUser>(
    IHttpContextService httpContextService,
    IRepository<TApiUser> repository
) : IApiUserService<TApiUser>
    where TApiUser : EntityGuid, IApiUser
{
    public Guid? GetAuthenticatedUserId() =>
        httpContextService.GetItem<AuthorizationResult>(AuthenticationDefaults.ApiContextAuthorizationKey)?.ApiUserId;

    public TApiUser? GetAuthenticatedUser() => repository.GetById(GetAuthenticatedUserId());

    public Task<TApiUser?> GetAuthenticatedUserAsync() => repository.GetByIdAsync(GetAuthenticatedUserId());
}