using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.TestTools.Data;

public static class SqliteInMemoryHelper
{
    public static SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return connection;
    }

    public static T CreateContext<T>(this SqliteConnection connection) where T : DbContext
    {
        var contextOptions = new DbContextOptionsBuilder<T>().UseSqlite(connection).Options;
        var context = (T)Activator.CreateInstance(typeof(T), contextOptions)! ?? throw new InvalidOperationException();
        context.Database.EnsureCreated();
        return context;
    }
}