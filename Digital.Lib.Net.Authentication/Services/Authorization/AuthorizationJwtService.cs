using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Digital.Lib.Net.Authentication.Exceptions;
using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Services;

namespace Digital.Lib.Net.Authentication.Services.Authorization;

public class AuthorizationJwtService(
    IRepository<User, DigitalContext> userRepository,
    IRepository<ApiToken, DigitalContext> apiTokenRepository,
    IHttpContextService httpContextService,
    IAuthenticationOptionService authenticationOptionService
) : IAuthorizationJwtService
{
    public string? GetRequestKey() => httpContextService.BearerToken;

    public AuthorizationResult AuthorizeUser(string? token)
    {
        var result = new AuthorizationResult();
        if (string.IsNullOrWhiteSpace(token))
            return result.AddError(new TokenNotFoundException());

        var handler = new JwtSecurityTokenHandler();
        try
        {
            handler.ValidateToken(token, authenticationOptionService.GetTokenParameters(), out _);
            var content = handler
                .ReadJwtToken(token)
                .Claims.First(c => c.Type == DefaultAuthenticationOptions.ContentClaimType)
                .Value;
            var decoded = JsonSerializer.Deserialize<TokenContent>(content);
            var user = userRepository.Get(u => decoded != null && u.Id == decoded.Id).FirstOrDefault();

            if (user is null)
                throw new InvalidTokenException();

            result.UserId = user.Id;
            result.Role = user.Role;
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }

    public AuthorizationResult AuthorizeRefreshToken(string? token)
    {
        var record = apiTokenRepository
            .Get(a => a.Key == (token ?? string.Empty))
            .FirstOrDefault();
        return record is null
            ? new AuthorizationResult().AddError(new InvalidTokenException())
            : AuthorizeUser(record.Key);
    }
}