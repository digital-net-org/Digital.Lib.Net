using System.ComponentModel.DataAnnotations;

namespace Digital.Lib.Net.Sdk.Services.Options;

public enum OptionAccessor
{
    [Display(Name="JwtRefreshExpiration")]
    JwtRefreshExpiration,
    [Display(Name="JwtBearerExpiration")]
    JwtBearerExpiration,
    [Display(Name="JwtSecret")]
    JwtSecret,
    [Display(Name="FileSystemPath")]
    FileSystemPath,
}