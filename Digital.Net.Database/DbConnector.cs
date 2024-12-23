using Digital.Net.Core.Environment;
using Digital.Net.Database.Options;
using Digital.Net.Database.Services;
using Digital.Net.Database.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Digital.Net.Database;

public static class DbConnector
{
    /// <summary>
    ///     Adds a database connector to the application. The database engine is set to PostgreSql by default.
    ///     Use the <see cref="DigitalDatabaseOptions" /> to configure the database engine and connection string.
    /// </summary>
    /// <typeparam name="TContext">The database context to be used.</typeparam>
    /// <param name="builder">The web application builder.</param>
    /// <param name="buildOptions">The action to build the database options.</param>
    /// <returns>The web application builder.</returns>
    public static WebApplicationBuilder AddDbConnector<TContext>(
        this WebApplicationBuilder builder,
        Action<DigitalDatabaseOptions> buildOptions
    )
        where TContext : DbContext
    {
        builder.Services.Configure(buildOptions);
        builder.Services.AddScoped<IDataAccessor, DataAccessor>();

        if (AspNetEnv.IsTest)
            builder.Services.AddEntityFrameworkProxies();

        builder.Services.AddDbContext<TContext>((provider, opts) =>
        {
            var options = provider.GetRequiredService<IOptions<DigitalDatabaseOptions>>().Value;
            if (AspNetEnv.IsTest || options.DatabaseEngine is DatabaseEngine.SqLiteInMemory)
                opts.UseSqlite(DatabaseUtils.InMemorySqliteConnection);
            else if (options.DatabaseEngine is DatabaseEngine.PostgreSql)
                opts.UseNpgsql(options.ConnectionString, b => b.MigrationsAssembly(options.MigrationAssembly));
            else
                throw new NotImplementedException("Database engine is not supported");

            opts.UseLazyLoadingProxies();
        }, ServiceLifetime.Transient);

        return builder;
    }
}