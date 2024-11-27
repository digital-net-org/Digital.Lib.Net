using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Net.Mvc;

public static class DigitalMvcInjector
{
    public static IServiceCollection AddDigitalMvc(this IServiceCollection services) =>
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
}