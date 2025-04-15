using System.Security.Authentication;
using Digital.Lib.Net.Authentication.Events;
using Digital.Lib.Net.Authentication.Exceptions;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Authentication.Services.Authorization;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Events.Services;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Authentication.Services.Authentication;

public class AuthenticationService(
    IAuthenticationOptionService authenticationOptionService,
    IAuthenticationJwtService authenticationJwtService,
    IAuthorizationJwtService authorizationJwtService,
    IEventService eventService,
    IRepository<User, DigitalContext> userRepository,
    IRepository<ApiToken, DigitalContext> tokenRepository,
    IRepository<Event, DigitalContext> eventRepository
) : IAuthenticationService
{
    private async Task<int> GetLoginAttemptCountAsync(User? user = null, string? ipAddress = null)
    {
        if (user is null)
            return 0;
        ipAddress ??= string.Empty;
        var threshold = DateTime.UtcNow.Subtract(authenticationOptionService.GetMaxLoginAttemptsThreshold());
        return await eventRepository.CountAsync(
            e =>
                e.CreatedAt > threshold
                && e.Name == AuthenticationEvents.Login
                && e.State == EventState.Failed
                && e.IpAddress == ipAddress
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
        else if (result.Value is null)
            result.AddError(new InvalidCredentialsException());
        else if (!result.Value.IsActive)
            result.AddError(new InactiveUserException());
        else if (!PasswordUtils.VerifyPassword(result.Value, password))
            result.AddError(new InvalidCredentialsException());
        return result;
    }

    public async Task<Result<(Guid, string)>> LoginAsync(
        string login,
        string password,
        string? userAgent = null,
        string? ipAddress = null
    )
    {
        var result = new Result<(Guid, string)>((Guid.Empty, string.Empty));
        userAgent ??= string.Empty;
        var userResult = await ValidateCredentialsAsync(login, password);
        var state = userResult.HasError() ? EventState.Failed : EventState.Success;

        result.Merge(userResult);

        await eventService.RegisterEventAsync(
            AuthenticationEvents.Login,
            state,
            result,
            userResult.Value?.Id,
            login,
            userAgent,
            ipAddress
        );

        if (result.HasError())
            return result;

        result.Value = (
            userResult.Value.Id,
            authenticationJwtService.GenerateBearerToken(userResult.Value!.Id, userAgent)
        );
        return result;
    }

    public async Task<Result<(Guid, string)>> RefreshTokensAsync(string? refreshToken, string? userAgent = null)
    {
        var result = new Result<(Guid, string)>((Guid.Empty, string.Empty));
        var tokenResult = authorizationJwtService.AuthorizeRefreshToken(refreshToken);
        result.Merge(tokenResult);
        if (result.HasError())
            return result;

        await authenticationJwtService.RevokeTokenAsync(refreshToken!);
        result.Value = (
            tokenResult.UserId,
            authenticationJwtService.GenerateBearerToken(tokenResult.UserId, userAgent)
        );
        return result;
    }

    public async Task<Result> LogoutAsync(string? refreshToken, Guid? userId)
    {
        var result = new Result();
        if (refreshToken is null)
            return result.AddError(new AuthenticationException());

        await authenticationJwtService.RevokeTokenAsync(refreshToken);
        await eventService.RegisterEventAsync(
            AuthenticationEvents.Logout,
            EventState.Success,
            null,
            userId
        );
        return result;
    }

    public async Task<Result> LogoutAllAsync(string? refreshToken)
    {
        var result = new Result();
        var userId = tokenRepository.Get(u => u.Key == refreshToken).FirstOrDefault()?.UserId;
        if (userId is null)
            return result.AddError(new AuthenticationException());

        await authenticationJwtService.RevokeAllTokensAsync(userId.Value);
        await eventService.RegisterEventAsync(
            AuthenticationEvents.LogoutAll,
            EventState.Success,
            null,
            userId
        );
        return result;
    }
}