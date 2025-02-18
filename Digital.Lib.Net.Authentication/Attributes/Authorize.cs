using Digital.Lib.Net.Authentication.Exceptions;
using Digital.Lib.Net.Authentication.Services.Authorization;
using Digital.Lib.Net.Authentication.Extensions;
using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Mvc.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Authentication.Attributes;

/// <summary>
///     Used to authorize a User to use a controller.
///     The authorization can be done using an API Key, a JWT token, or both.
///     The authorization result is stored in the Api Context.
/// </summary>
/// <param name="type">The type of authorization to use.</param>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAttribute(AuthorizeType type, UserRole role = UserRole.User) : Attribute, IAuthorizationFilter
{
    private AuthorizeType Type { get; } = type;
    public UserRole Role { get; set; } = role;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var result = new AuthorizationResult();

        if (Type.HasFlag(AuthorizeType.ApiKey))
            result.Merge(AuthorizeApiKey(context));
        if (Type.HasFlag(AuthorizeType.Jwt) && result.IsAuthorized is false)
            result.Merge(AuthorizeJwt(context));
        if (result.Role < Role)
            result.Forbid();
        if (!result.HasError() || result is { IsAuthorized: true, IsForbidden: false })
        {
            var contextService = context.HttpContext.RequestServices.GetRequiredService<IHttpContextService>();
            contextService.AddItem(DefaultAuthenticationOptions.ApiContextAuthorizationKey, result);
            return;
        }
        context.RejectAuthorization(result.IsForbidden ? 403 : 401);
    }

    private AuthorizationResult AuthorizeApiKey(AuthorizationFilterContext context)
    {
        var result = new AuthorizationResult();
        var service = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationApiKeyService>();
        var apiKey = service.GetRequestKey();

        if (Type.HasFlag(AuthorizeType.Jwt) && apiKey is null)
            return result;

        result.Merge(service.AuthorizeUser(apiKey));
        if (!result.HasError())
            result.Authorize();

        return result;
    }

    private AuthorizationResult AuthorizeJwt(AuthorizationFilterContext context)
    {
        var result = new AuthorizationResult();
        var service = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationJwtService>();
        var token = service.GetRequestKey();

        if (token is null)
            return result.AddError(new TokenNotFoundException());

        result.Merge(service.AuthorizeUser(token));
        if (!result.HasError())
            result.Authorize();

        return result;
    }
}