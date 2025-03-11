using Digital.Lib.Net.Core.Application;
using Digital.Lib.Net.Core.Application.Settings;
using Digital.Lib.Net.Core.Environment;
using Digital.Lib.Net.Core.Random;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Digital.Lib.Net.TestTools.Integration;

public static class AppFactorySettings
{
    private static string DbPath => Path.Combine(
        Path.GetTempPath(),
        $"sqlite_db_{Randomizer.GenerateRandomString(Randomizer.AnyNumber, 8)}.db"
    );

    public static Dictionary<string, string?> TestSettings => new()
    {
        { AppSettings.Domain, "domain.test" },
        { AppSettings.ConnectionString, $"Data Source={DbPath}" },
        { AppSettings.UseSqlite, "true" },
        { AppSettings.AuthJwtSecret, "superLongSecretThatNeedsToBeSuperLongAndSecure" }
    };

    public static IWebHostBuilder UseTestConfiguration(this IWebHostBuilder hostBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .AddAppSettings()
            .AddInMemoryCollection(TestSettings)
            .Build();
        hostBuilder.UseConfiguration(configuration);
        return hostBuilder;
    }

    public static IWebHostBuilder UseTestEnvironment(this IWebHostBuilder hostBuilder)
    {
        hostBuilder.UseEnvironment(AspNetEnv.Test);
        return hostBuilder;
    }
}