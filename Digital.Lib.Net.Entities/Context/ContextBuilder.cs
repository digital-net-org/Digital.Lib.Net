using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Entities.Context;

public static class ContextBuilder
{
    public static T Build<T>(string connStr)
        where T : DbContext
    {
        var optionsBuilder = new DbContextOptionsBuilder<T>();
        optionsBuilder.UseNpgsql(connStr);
        return (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options)!;
    }

    public static string GetConnectionString(string?[]? args)
    {
        var result = args is not null && args.Length > 0 ? args[0] : null;
        return result ?? throw new Exception("No connection string specified in args.");
    }
}
