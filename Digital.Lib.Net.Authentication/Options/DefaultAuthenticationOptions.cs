namespace Digital.Lib.Net.Authentication.Options;

public static class DefaultAuthenticationOptions
{
    public const long DefaultRefreshTokenExpiration = 1800000;
    public const long DefaultAccessTokenExpiration = 300000;

    public const string ContentClaimType = "Content";
    public const string ApiContextAuthorizationKey = "AuthorizationResult";
    public const int SaltSize = 16;
    public const int MaxConcurrentSessions = 5;
    public const int MaxLoginAttempts = 3;
    public const long MaxLoginAttemptsThreshold = 900000;
}