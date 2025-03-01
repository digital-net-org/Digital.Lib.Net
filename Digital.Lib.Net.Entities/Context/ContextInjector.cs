using Digital.Lib.Net.Core.Application.Settings;
using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Entities.Context;

public static class ContextInjector
{
    public static WebApplicationBuilder AddNpgsqlContext<T>(
        this WebApplicationBuilder builder,
        string? accessor = null
    )
        where T : DbContext
    {
        accessor ??= "Default";
        var connStr = builder.Configuration.GetOrThrow<string>($"{AppSettings.ConnectionStrings}:{accessor}");
        builder.Services.AddDbContext<T>(options =>
        {
            options.UseNpgsql(connStr);
        });
        return builder;
    }
}