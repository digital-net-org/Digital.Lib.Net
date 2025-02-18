namespace Digital.Lib.Net.Core.Application.Settings;

public static class AppSettings
{
    public const string Domain = "Domain";
    public const string CorsAllowedOrigins = "CorsAllowedOrigins";

    public const string FileSystemPath = "FileSystem:Path";
    public const string FileSystemMaxAvatarSize = "FileSystem:MaxAvatarSize";

    public const string AuthJwtRefreshExpiration = "Auth:JwtRefreshExpiration";
    public const string AuthJwtBearerExpiration = "Auth:JwtBearerExpiration";
    public const string AuthJwtSecret = "Auth:JwtSecret";
    public const string AuthPasswordRegex = "Auth:PasswordRegex";
}