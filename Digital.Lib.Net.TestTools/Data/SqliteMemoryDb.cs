using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.TestTools.Data;

public sealed class SqliteMemoryDb<T> : IDisposable where T : DbContext
{
    private readonly SqliteConnection _connection;

    public SqliteMemoryDb()
    {
        _connection = SqliteUtils.InMemorySqliteConnection;
        var contextOptions = new DbContextOptionsBuilder<T>().UseSqlite(_connection).Options;
        _connection.Open();
        Context = (T)Activator.CreateInstance(typeof(T), contextOptions)! ?? throw new InvalidOperationException();
        Context.Database.EnsureCreated();
    }

    public T Context { get; }

    public void Dispose() => _connection.Close();
}