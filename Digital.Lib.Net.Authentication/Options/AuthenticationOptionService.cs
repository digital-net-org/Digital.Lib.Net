using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Digital.Lib.Net.Authentication.Options;

public class AuthenticationOptionService(IOptions<AuthenticationOptions> options) : IAuthenticationOptionService
{
    public Regex PasswordRegex => options.Value.PasswordConfig.PasswordRegex;
    public string CookieName => options.Value.JwtTokenConfig.CookieName;
    public string ApiKeyHeaderAccessor => options.Value.ApiKeyConfig.HeaderAccessor;

    public TimeSpan GetMaxLoginAttemptsThreshold() =>
        TimeSpan.FromMilliseconds(DefaultAuthenticationOptions.MaxLoginAttemptsThreshold);

    public DateTime GetRefreshTokenExpirationDate(DateTime? from = null) =>
        (from ?? DateTime.UtcNow).AddMilliseconds(options.Value.JwtTokenConfig.RefreshTokenExpiration);

    public DateTime GetBearerTokenExpirationDate(DateTime? from = null) =>
        (from ?? DateTime.UtcNow).AddMilliseconds(options.Value.JwtTokenConfig.AccessTokenExpiration);

    public TokenValidationParameters GetTokenParameters() => new()
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = options.Value.JwtTokenConfig.Issuer,
        ValidAudience = options.Value.JwtTokenConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.JwtTokenConfig.Secret)),
        ClockSkew = TimeSpan.Zero
    };
}