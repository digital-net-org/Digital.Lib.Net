using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Entities.Models;

namespace Digital.Lib.Net.Authentication.Services.Authentication.ApiUsers;

public interface IApiUserService<TApiUser>
    where TApiUser : EntityGuid, IApiUser
{
    public Guid? GetAuthenticatedUserId();

    public TApiUser? GetAuthenticatedUser();

    public Task<TApiUser?> GetAuthenticatedUserAsync();
}