using Digital.Lib.Net.Core.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Entities.Context;

public static class ContextUtils
{
    public static async Task ApplyMigrationsAsync<T>(this WebApplication app)
        where T : DbContext
    {
        if (AspNetEnv.IsTest)
            return;

        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>();
        await context.Database.MigrateAsync();
    }
}
