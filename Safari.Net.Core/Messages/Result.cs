namespace Safari.Net.Core.Messages;

/// <summary>
///     A class to hold the result of a message. Can be created using either an exception or an enum.
/// </summary>
public class Result
{
    public List<ResultMessage> Errors { get; init; } = [];
    public List<ResultMessage> Infos { get; init; } = [];

    public bool HasError => Errors.Count > 0;

    public Result Merge(Result result)
    {
        Errors.AddRange(result.Errors);
        Infos.AddRange(result.Infos);
        return this;
    }

    public Result AddError(Exception ex, Enum? message = null)
    {
        Errors.Add(new ResultMessage(ex, message));
        return this;
    }

    public Result AddError(Enum message)
    {
        Errors.Add(new ResultMessage(message));
        return this;
    }

    public Result AddInfo(Enum message)
    {
        Infos.Add(new ResultMessage(message));
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

    public new Result<T> AddError(Exception ex, Enum? message = null)
    {
        Errors.Add(new ResultMessage(ex, message));
        return this;
    }

    public new Result<T> AddError(Enum message)
    {
        Errors.Add(new ResultMessage(message));
        return this;
    }

    public new Result<T> AddInfo(Enum message)
    {
        Infos.Add(new ResultMessage(message));
        return this;
    }
}