using Digital.Lib.Net.Authentication.Controllers.Models;
using Digital.Lib.Net.Authentication.Exceptions;
using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Authentication.Services.Authentication;
using Digital.Lib.Net.Core.Extensions.ExceptionUtilities;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Lib.Net.Authentication.Controllers;

public abstract class AuthenticationController<TApiUser>(
    IAuthenticationService<TApiUser> authenticationService
) : ControllerBase
    where TApiUser : EntityGuid, IApiUser
{
    [HttpPost("login")]
    public virtual async Task<ActionResult<Result<string>>> Login([FromBody] LoginPayload request)
    {
        var result = await authenticationService.Login(request.Login, request.Password);

        if (result.Errors.Any(e => e.Reference == new AuthenticationTooManyAttemptsException().GetReference()))
            return StatusCode(429);
        if (result.HasError() || result.Value is null)
            return Unauthorized();
        return Ok(result);
    }

    [HttpPost("refresh")]
    public virtual ActionResult<Result<string>> RefreshTokens()
    {
        var result = authenticationService.RefreshTokens();
        return result.HasError() ? Unauthorized() : Ok(result);
    }

    [HttpPost("logout")]
    public virtual async Task<IActionResult> Logout()
    {
        await authenticationService.Logout();
        return NoContent();
    }

    [HttpPost("logout-all")]
    public virtual async Task<IActionResult> LogoutAll()
    {
        await authenticationService.LogoutAll();
        return NoContent();
    }
}