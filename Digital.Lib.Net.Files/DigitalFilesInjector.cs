using Digital.Lib.Net.Files.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Files;

public static class DigitalFilesInjector
{
    public static WebApplicationBuilder AddDigitalFilesServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        return builder;
    }
}