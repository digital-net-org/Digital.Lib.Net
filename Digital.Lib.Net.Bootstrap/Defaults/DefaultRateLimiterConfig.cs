using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;

namespace Digital.Lib.Net.Bootstrap.Defaults;

public static class DefaultRateLimiterConfig
{
    public static WebApplicationBuilder AddRateLimiter(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("Default", opts =>
            {
                opts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opts.PermitLimit = 1000;
                opts.QueueLimit = 1000;
                opts.Window = TimeSpan.FromMilliseconds(1000);
            });
        });
        return builder;
    }
}