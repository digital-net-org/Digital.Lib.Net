using Microsoft.EntityFrameworkCore.Design;

namespace Digital.Lib.Net.Entities.Context;

public class ContextFactory : IDesignTimeDbContextFactory<DigitalContext>
{
    public DigitalContext CreateDbContext(string[] args)
    {
        var opts = ContextBuilder.GetConnectionString(args);
        return ContextBuilder.Build<DigitalContext>(opts);
    }
}
