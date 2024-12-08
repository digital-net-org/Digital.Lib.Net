using Digital.Net.Core.Messages;

namespace Digital.Net.Database.Services;

public interface IDataAccessor
{
    public Result ExecuteSql(string query);
}