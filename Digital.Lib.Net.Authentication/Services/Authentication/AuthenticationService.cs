using System.Security.Authentication;
using Digital.Lib.Net.Authentication.Exceptions;
using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Authentication.Services.Authorization;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Events.Services;
using Digital.Lib.Net.Mvc.Services;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Authentication.Services.Authentication;

public class AuthenticationService(
    IHttpContextService httpContextService,
    IAuthenticationOptionService authenticationOptionService,
    IAuthenticationJwtService authenticationJwtService,
    IAuthorizationJwtService authorizationJwtService,
    IEventService eventService,
    IRepository<User, DigitalContext> userRepository,
    IRepository<ApiToken, DigitalContext> tokenRepository,
    IRepository<Event, DigitalContext> eventRepository
) : IAuthenticationService
{
    public Guid? GetAuthenticatedUserId() =>
        httpContextService
            .GetItem<AuthorizationResult>(DefaultAuthenticationOptions.ApiContextAuthorizationKey)
            ?.UserId;
    public User? GetAuthenticatedUser() =>
        userRepository.GetById(GetAuthenticatedUserId());
    public async Task<User?> GetAuthenticatedUserAsync() =>
        await userRepository.GetByIdAsync(GetAuthenticatedUserId());

    public async Task<Result<User>> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var result = new Result<User>();

        if (!PasswordUtils.VerifyPassword(user, currentPassword))
            return result.AddError(new InvalidCredentialsException());
        if (!authenticationOptionService.PasswordRegex.IsMatch(newPassword))
            return result.AddError(new PasswordMalformedException());

        user.Password = PasswordUtils.HashPassword(newPassword);
        userRepository.Update(user);
        await userRepository.SaveAsync();
        return result;
    }

    public async Task<int> GetLoginAttemptCountAsync(User? user = null)
    {
        if (user is null)
            return 0;
        var threshold = DateTime.UtcNow.Subtract(authenticationOptionService.GetMaxLoginAttemptsThreshold());
        return await eventRepository.CountAsync(
            e =>
                e.CreatedAt > threshold
                && e.Name == AuthenticationEvents.Login
                && e.State == EventState.Failed
                && e.IpAddress == httpContextService.IpAddress
                && e.UserId == user.Id
        );
    }

    public async Task<Result<User>> ValidateCredentialsAsync(string login, string password)
    {
        var result = new Result<User>
        {
            Value = await userRepository.Get(u => u.Login == login).FirstOrDefaultAsync()
        };

        if (await GetLoginAttemptCountAsync(result.Value) >= DefaultAuthenticationOptions.MaxLoginAttempts)
            result.AddError(new TooManyAttemptsException());
        if (result.Value is null)
            result.AddError(new InvalidCredentialsException());
        else if (!result.Value.IsActive)
            result.AddError(new InactiveUserException());
        else if (!PasswordUtils.VerifyPassword(result.Value, password))
            result.AddError(new InvalidCredentialsException());
        return result;
    }

    public async Task<Result<string>> LoginAsync(string login, string password)
    {
        var result = new Result<string>();
        var userResult = await ValidateCredentialsAsync(login, password);
        var state = userResult.HasError() ? EventState.Failed : EventState.Success;

        result.Merge(userResult);

        await eventService.RegisterEventAsync(
            AuthenticationEvents.Login,
            state,
            result,
            userResult.Value?.Id,
            login
        );

        if (result.HasError())
            return result;

        result.Value = authenticationJwtService.GenerateBearerToken(userResult.Value!.Id);
        httpContextService.SetResponseCookie(
            authenticationJwtService.GenerateRefreshToken(userResult.Value.Id),
            authenticationOptionService.CookieName,
            authenticationOptionService.GetRefreshTokenExpirationDate()
        );
        return result;
    }

    public Result<string> RefreshTokens()
    {
        var token = httpContextService.Request.Cookies[authenticationOptionService.CookieName];
        var result = new Result<string>();

        var tokenResult = authorizationJwtService.AuthorizeRefreshToken(token);
        result.Merge(tokenResult);
        if (result.HasError())
            return result;

        httpContextService.SetResponseCookie(
            authenticationJwtService.GenerateRefreshToken(tokenResult.UserId),
            authenticationOptionService.CookieName,
            authenticationOptionService.GetRefreshTokenExpirationDate()
        );
        result.Value = authenticationJwtService.GenerateBearerToken(tokenResult.UserId);
        return result;
    }

    public async Task<Result> LogoutAsync()
    {
        var result = new Result();
        var refreshToken = httpContextService.Request.Cookies[authenticationOptionService.CookieName];
        if (refreshToken is null)
            return result.AddError(new AuthenticationException());

        await authenticationJwtService.RevokeTokenAsync(refreshToken);
        httpContextService.Response.Cookies.Delete(authenticationOptionService.CookieName);

        await eventService.RegisterEventAsync(
            AuthenticationEvents.Logout,
            EventState.Success,
            null,
            GetAuthenticatedUserId()
        );
        return result;
    }

    public async Task<Result> LogoutAllAsync()
    {
        var result = new Result();
        var refreshToken = httpContextService.Request.Cookies[authenticationOptionService.CookieName];
        var userId = tokenRepository.Get(u => u.Key == refreshToken).FirstOrDefault()?.UserId;
        if (userId is null)
            return result.AddError(new AuthenticationException());

        await authenticationJwtService.RevokeAllTokensAsync(userId.Value);
        httpContextService.Response.Cookies.Delete(authenticationOptionService.CookieName);

        await eventService.RegisterEventAsync(
            AuthenticationEvents.LogoutAll,
            EventState.Success,
            null,
            userId
        );
        return result;
    }
}