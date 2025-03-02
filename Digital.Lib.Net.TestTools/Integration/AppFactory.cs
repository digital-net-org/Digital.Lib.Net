using Digital.Lib.Net.Bootstrap.Services;
using Digital.Lib.Net.TestTools.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.TestTools.Integration;

public class AppFactory<T> : WebApplicationFactory<T> where T : class
{
    private readonly SqliteConnection _connection;

    public AppFactory()
    {
        _connection = SqliteUtils.InMemorySqliteConnection;
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder
            .UseTestEnvironment()
            .UseTestConfiguration()
            .ConfigureTestServices(s => { ReplaceDatabasesWithMemory(s, _connection); });

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection.Dispose();
    }

    public static void ReplaceDatabasesWithMemory(IServiceCollection services, SqliteConnection connection)
    {
        var descriptors = services.GetEfDescriptors();

        foreach (var descriptor in descriptors)
            services.Remove(descriptor);

        foreach (var descriptor in descriptors.GetContextDescriptors())
        {
            descriptor
                .GetAddDbContextMethod()
                .Invoke(null, [services, (Action<DbContextOptionsBuilder>)(o => o.UseSqlite(connection)), null, null]);
            services.EnsureDbContextCreated(descriptor);
        }
    }
}