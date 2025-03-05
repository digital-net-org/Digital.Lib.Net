using Digital.Lib.Net.Core.Application;
using Digital.Lib.Net.Core.Environment;
using Digital.Lib.Net.TestTools.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Digital.Lib.Net.TestTools.Integration;

public static class AppFactorySettings
{
    public static readonly Dictionary<string, string?> TestSettings = new()
    {
        { "Domain", "domain.test" },
        { "ConnectionStrings:Default", SqliteUtils.InMemoryConnectionString },
        { "Auth:JwtSecret", "superLongSecretThatNeedsToBeSuperLongAndSecure" },
        { "Auth:JwtRefreshExpiration", "60000" },
        { "Auth:JwtBearerExpiration", "5000" }
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