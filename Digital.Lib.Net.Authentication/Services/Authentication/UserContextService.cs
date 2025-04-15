using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Services;

namespace Digital.Lib.Net.Authentication.Services.Authentication;

public class UserContextService(
    IRepository<User, DigitalContext> userRepository,
    IHttpContextService httpContextService
) : IUserContextService
{
    public Guid GetUserId() =>
        httpContextService
            .GetItem<AuthorizationResult>(DefaultAuthenticationOptions.ApiContextAuthorizationKey)
            ?.UserId ?? throw new UnauthorizedAccessException();

    public User GetUser() => userRepository.GetById(GetUserId());
}