using Digital.Lib.Net.Core.Application;
using Digital.Lib.Net.Mvc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Mvc;

public static class DigitalMvcInjector
{
    public static IServiceCollection AddDigitalMvc(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddScoped<IHttpContextService, HttpContextService>();

        if (!services.IsInjected(typeof(IHttpContextAccessor)))
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }

    public static WebApplicationBuilder AddDigitalMvc(this WebApplicationBuilder builder)
    {
        builder.Services.AddDigitalMvc();
        return builder;
    }
}