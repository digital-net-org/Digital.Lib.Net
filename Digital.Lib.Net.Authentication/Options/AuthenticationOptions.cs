using Digital.Lib.Net.Authentication.Options.Config;

namespace Digital.Lib.Net.Authentication.Options;

public class AuthenticationOptions
{
    public ApiKeyConfig ApiKeyConfig { get; private set; } = new();

    /// <summary>
    ///     Set Api keys options.
    /// </summary>
    public AuthenticationOptions SetApiKeyOptions(ApiKeyConfig apiKeyConfig)
    {
        ApiKeyConfig = apiKeyConfig;
        return this;
    }

    public PasswordConfig PasswordConfig { get; private set; } = new();

    /// <summary>
    ///     Set the password validation and security options.
    /// </summary>
    public AuthenticationOptions SetPasswordOptions(PasswordConfig config)
    {
        PasswordConfig = config;
        return this;
    }

    public JwtTokenConfig JwtTokenConfig { get; private set; } = new();

    /// <summary>
    ///     Set the JWT token configuration options.
    /// </summary>
    public AuthenticationOptions SetJwtTokenOptions(JwtTokenConfig config)
    {
        JwtTokenConfig = config;
        return this;
    }
}