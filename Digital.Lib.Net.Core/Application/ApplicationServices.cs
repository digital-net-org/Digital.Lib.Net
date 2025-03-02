using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Core.Application;

public static class ApplicationServices
{
    public static void ConfigureOptions<T>(this IServiceCollection services, Action<T>? buildOptions = null)
        where T : class, new()
    {
        if (buildOptions is null)
            services.Configure<T>(_ => { });
        else
            services.Configure(buildOptions);
    }
}
