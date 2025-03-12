using Digital.Lib.Net.Core.Random;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Sdk.Services.Options;

public static class ApplicationDefaults
{
    public static readonly List<(string, OptionAccessor, string)> Settings =
    [
        (AppSettings.DefaultsJwtBearerExpiration, OptionAccessor.JwtBearerExpiration, "300000"),
        (AppSettings.DefaultsJwtRefreshExpiration, OptionAccessor.JwtRefreshExpiration, "1800000"),
        (AppSettings.DefaultsJwtSecret, OptionAccessor.JwtSecret, Randomizer.GenerateRandomString(Randomizer.AnyCharacter, 64)),
        (AppSettings.DefaultsFileSystemPath, OptionAccessor.FileSystemPath, "/digital_net_storage"),
    ];
}