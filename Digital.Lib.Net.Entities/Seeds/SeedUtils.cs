using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Entities.Seeds;

public static class SeedUtils
{
    public static async Task ApplyDataSeedsAsync(this WebApplication app)
    {
        var seeds = app.Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IEnumerable<ISeed>>();

        foreach (var seed in seeds)
            await seed.ApplySeed();
    }
}