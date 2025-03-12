using System.Text;
using Digital.Lib.Net.Sdk.Services.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Digital.Lib.Net.Authentication.Options;

public class AuthenticationOptionService(
    IOptions<AuthenticationOptions> options,
    IAppOptionService appOptionService
) : IAuthenticationOptionService
{
    public string CookieName => options.Value.JwtTokenConfig.CookieName;
    public string ApiKeyHeaderAccessor => options.Value.ApiKeyConfig.HeaderAccessor;

    public TimeSpan GetMaxLoginAttemptsThreshold() =>
        TimeSpan.FromMilliseconds(DefaultAuthenticationOptions.MaxLoginAttemptsThreshold);

    public DateTime GetRefreshTokenExpirationDate(DateTime? from = null) =>
        (from ?? DateTime.UtcNow).AddMilliseconds(appOptionService.Get<long>(OptionAccessor.JwtRefreshExpiration));

    public DateTime GetBearerTokenExpirationDate(DateTime? from = null) =>
        (from ?? DateTime.UtcNow).AddMilliseconds(appOptionService.Get<long>(OptionAccessor.JwtBearerExpiration));

    public TokenValidationParameters GetTokenParameters() => new()
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = options.Value.JwtTokenConfig.Issuer,
        ValidAudience = options.Value.JwtTokenConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(appOptionService.Get<string>(OptionAccessor.JwtSecret))
        ),
        ClockSkew = TimeSpan.Zero
    };
}