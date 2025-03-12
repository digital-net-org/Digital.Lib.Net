using System.Threading.Tasks;
using Digital.Lib.Net.Entities;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.ApplicationOptions;
using Digital.Lib.Net.Entities.Models.Avatars;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Seeds;
using Digital.Lib.Net.Sdk.Bootstrap;
using Digital.Lib.Net.Sdk.Services.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Sdk;

public static class DigitalSdkInjector
{
    /// <summary>
    ///     Validate application settings, add the DigitalContext and register Entities services for them and register
    ///     AppOptionService and initiate options in database.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDigitalSdk(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddAppSettings();
        builder
            .ValidateApplicationSettings()
            .AddDatabaseContext<DigitalContext>()
            .ApplyMigrations<DigitalContext>()
            .ApplyDataSeeds();

        builder.Services
            .AddDigitalEntities<ApiKey>()
            .AddDigitalEntities<ApiToken>()
            .AddDigitalEntities<ApplicationOption>()
            .AddDigitalEntities<Avatar>()
            .AddDigitalEntities<Document>()
            .AddDigitalEntities<Event>()
            .AddDigitalEntities<User>()
            .AddScoped<IAppOptionService, AppOptionService>();
        builder.SetAppDefaults();
        return builder;
    }

    private static void SetAppDefaults(this WebApplicationBuilder builder) =>
        builder.Services
            .BuildServiceProvider()
            .GetService<IAppOptionService>()?
            .SettingsInit();
}