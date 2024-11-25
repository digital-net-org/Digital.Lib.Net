namespace Digital.Net.Core.Errors;

public static class TryCatchUtilities
{
    /// <summary>
    ///     Try to execute a function and return the result or null if an exception is thrown.
    ///     Use this method when you want to ignore exceptions and return null instead.
    /// </summary>
    /// <param name="func">The function to execute.</param>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <returns>The result of the function if it executes successfully, otherwise null.</returns>
    public static T? TryOrNull<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    ///     Try to execute a function and return the result or null if an exception is thrown.
    ///     Use this method when you want to ignore exceptions and return null instead.
    /// </summary>
    /// <param name="func">The function to execute.</param>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <returns>The result of the function if it executes successfully, otherwise null.</returns>
    public static async Task<T?> TryOrNullAsync<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch
        {
            return default;
        }
    }
}