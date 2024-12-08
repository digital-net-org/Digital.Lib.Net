namespace Digital.Net.Database.Options;

public class DigitalDatabaseOptions
{
    public string ConnectionString { get; private set; } = string.Empty;
    public string MigrationAssembly { get; private set; } = string.Empty;
    public DatabaseEngine DatabaseEngine { get; private set; } = DatabaseEngine.PostgreSql;

    /// <summary>
    ///     Sets the connection string for the database.
    /// </summary>
    /// <param name="connectionString"></param>
    public DigitalDatabaseOptions SetConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
        return this;
    }

    /// <summary>
    ///     Sets the migration assembly for the database.
    /// </summary>
    /// <param name="migrationAssembly"></param>
    public DigitalDatabaseOptions SetMigrationAssembly(string migrationAssembly)
    {
        MigrationAssembly = migrationAssembly;
        return this;
    }

    /// <summary>
    ///     Sets the database engine for the database.
    /// </summary>
    /// <param name="databaseEngine"></param>
    public DigitalDatabaseOptions SetDatabaseEngine(DatabaseEngine databaseEngine)
    {
        DatabaseEngine = databaseEngine;
        return this;
    }
}