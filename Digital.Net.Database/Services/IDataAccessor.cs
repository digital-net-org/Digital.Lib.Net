using Digital.Net.Core.Messages;

namespace Digital.Net.Database.Services;

public interface IDataAccessor
{
    /// <summary>
    ///     Executes a SQL query on the database.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public Result ExecuteSql(string query);
}