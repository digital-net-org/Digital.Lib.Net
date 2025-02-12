using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Database.Options;
using Digital.Lib.Net.Database.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Digital.Lib.Net.Database.Services;

public class DataAccessor(IOptions<DigitalDatabaseOptions> options) : IDataAccessor
{
    private string ConnectionString => options.Value.ConnectionString;
    private DatabaseEngine DatabaseEngine => options.Value.DatabaseEngine;
    public Result ExecuteSql(string query)
    {
        var result = new Result();
        try
        {
            if (DatabaseEngine is DatabaseEngine.PostgreSql)
                result = ExecutePostgreSql(query);
            else if (DatabaseEngine is DatabaseEngine.SqLiteInMemory)
                result = ExecuteSqLiteInMemory(query);
            else
                throw new NotSupportedException("Database engine is not supported");
        }
        catch (Exception ex)
        {
            result.AddError(ex);
        }

        return result;
    }

    private Result ExecutePostgreSql(string query)
    {
        var result = new Result();
        using var connection = new NpgsqlConnection(ConnectionString);
        connection.Open();
        using var cmd = new NpgsqlCommand(query, connection);
        var affectedRows = cmd.ExecuteNonQuery();
        result.AddInfo($"Affected rows: {affectedRows}");
        connection.Close();
        return result;
    }

    private static Result ExecuteSqLiteInMemory(string query)
    {
        var result = new Result();
        using var connection = DatabaseUtils.InMemorySqliteConnection;
        connection.Open();
        using var cmd = new SqliteCommand(query, connection);
        var affectedRows = cmd.ExecuteNonQuery();
        result.AddInfo($"Affected rows: {affectedRows}");
        return result;
    }
}