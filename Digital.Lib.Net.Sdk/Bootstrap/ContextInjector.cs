using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Sdk.Services.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Sdk.Bootstrap;

public static class ContextInjector
{
    public static WebApplicationBuilder AddDatabaseContext<T>(this WebApplicationBuilder builder)
        where T : DbContext
    {
        var connectionString = builder.Configuration.GetOrThrow<string>($"{AppSettings.ConnectionString}");
        var useSqlite = builder.Configuration.Get<bool>(AppSettings.UseSqlite);

        builder.Services.AddDbContext<T>(options =>
        {
            if (useSqlite)
                options.UseSqlite(connectionString);
            else
                options.UseDigitalNpgsql<T>(connectionString);
        });
        return builder;
    }

    public static WebApplicationBuilder ApplyMigrations<T>(this WebApplicationBuilder builder)
        where T : DbContext
    {
        if (builder.Configuration.Get<bool>(AppSettings.UseSqlite))
            return builder;

        var context = builder.Services.BuildServiceProvider().GetRequiredService<T>();
        context.Database.Migrate();
        return builder;
    }
}