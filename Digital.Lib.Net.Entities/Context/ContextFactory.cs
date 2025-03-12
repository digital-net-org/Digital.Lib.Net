using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Digital.Lib.Net.Entities.Context;

public class ContextFactory : IDesignTimeDbContextFactory<DigitalContext>
{
    public DigitalContext CreateDbContext(string[] args)
    {
        var opts = GetConnectionString(args);
        return Build<DigitalContext>(opts);
    }

    private static T Build<T>(string connStr)
        where T : DbContext
    {
        var optionsBuilder = new DbContextOptionsBuilder<T>();
        optionsBuilder.UseNpgsql(connStr);
        return (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options)!;
    }

    private static string GetConnectionString(string?[]? args)
    {
        var result = args is not null && args.Length > 0 ? args[0] : null;
        return result ?? throw new Exception("No connection string specified in args.");
    }
}
