using Digital.Lib.Net.Core.Application.Settings;
using Microsoft.AspNetCore.Builder;

namespace Digital.Lib.Net.Sdk.Services.Options;

public static class ApplicationSettings
{
    public static WebApplicationBuilder ValidateApplicationSettings(this WebApplicationBuilder builder)
    {
        var mandatorySettings = new[]
        {
            AppSettings.Domain,
            AppSettings.ConnectionString,
        };

        foreach (var setting in mandatorySettings)
        {
            var value = builder.Configuration.GetSection(setting).Value;
            if (string.IsNullOrWhiteSpace(value))
                throw new NullReferenceException($"Missing mandatory configuration section: {setting}");
        }

        return builder;
    }
}