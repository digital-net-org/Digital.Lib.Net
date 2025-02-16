using Microsoft.Data.Sqlite;

namespace Digital.Lib.Net.TestTools.Data;

public static class SqliteUtils
{
    public const string InMemoryConnectionString = "Filename=:memory:";

    public static SqliteConnection InMemorySqliteConnection => new(InMemoryConnectionString);
}