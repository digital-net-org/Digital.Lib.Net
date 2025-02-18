namespace Digital.Lib.Net.Core.Messages;

/// <summary>
///     A class to hold the result of a message. Can be created using either an exception or an enum.
/// </summary>
public class Result
{
    public List<ResultMessage> Errors { get; init; } = [];
    public List<ResultMessage> Infos { get; init; } = [];

    public bool HasError() => Errors.Count > 0;

    public bool HasError<TException>() where TException : Exception
    {
        var result = Errors.Any(error => error.IsExceptionOfType<TException>());
        return result;
    }

    public Result Merge(Result result)
    {
        Errors.AddRange(result.Errors);
        Infos.AddRange(result.Infos);
        return this;
    }

    public Result AddError(Exception ex)
    {
        Errors.Add(new ResultMessage(ex));
        return this;
    }

    public Result AddInfo(string message)
    {
        Infos.Add(new ResultMessage(message));
        return this;
    }

    public Result Try(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            AddError(ex);
        }

        return this;
    }
}

public class Result<T> : Result where T : class
{
    public Result()
    {
    }

    public Result(T value)
    {
        Value = value;
    }

    public T? Value { get; set; }

    public new Result<T> Merge(Result result)
    {
        base.Merge(result);
        return this;
    }

    public new Result<T> AddError(Exception ex)
    {
        base.AddError(ex);
        return this;
    }

    public new Result<T> AddInfo(string message)
    {
        base.AddInfo(message);
        return this;
    }

    public new Result<T> Try(Action action)
    {
        base.Try(action);
        return this;
    }
}