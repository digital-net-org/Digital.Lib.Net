using Digital.Lib.Net.Authentication.Exceptions;
using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Services;

namespace Digital.Lib.Net.Authentication.Services.Authorization;

public class AuthorizationApiKeyService(
    IRepository<ApiKey, DigitalContext> apiKeyRepository,
    IRepository<User, DigitalContext> userRepository,
    IHttpContextService httpContextService,
    IAuthenticationOptionService optionService
) : IAuthorizationApiKeyService
{
    public string? GetRequestKey() => httpContextService.GetHeaderValue(optionService.ApiKeyHeaderAccessor);

    public AuthorizationResult AuthorizeUser(string? key)
    {
        var result = new AuthorizationResult();
        if (string.IsNullOrWhiteSpace(key))
            return result.AddError(new TokenNotFoundException());

        var authorization = apiKeyRepository.Get(k => k.Key == key).FirstOrDefault();
        if (authorization is null)
            return result.AddError(new InvalidTokenException());

        if (authorization.ExpiredAt is not null && authorization.ExpiredAt < DateTime.UtcNow)
            return result.AddError(new ExpiredTokenException());

        var user = userRepository.Get(u => u.Id == authorization.UserId).FirstOrDefault();
        if (user is null)
            return result.AddError(new InvalidTokenException());

        result.Authorize(user.Id);
        result.Role = user.Role;
        return result;
    }
}