using Safari.Net.Core.Messages;

namespace Safari.Net.Data.Entities;

public class QueryResult<T> : Result<T> where T : class
{
    public new QueryResult<T> AddError(Exception ex, Enum? message = null)
    {
        Errors.Add(new ResultMessage(ex, message));
        return this;
    }

    public new QueryResult<T> AddError(Enum message)
    {
        Errors.Add(new ResultMessage(message));
        return this;
    }


}
