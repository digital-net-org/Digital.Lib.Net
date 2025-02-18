using Digital.Lib.Net.Core.Application.Settings;
using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Digital.Lib.Net.Files.Options;
using Digital.Lib.Net.Files.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Files;

public static class DigitalFilesInjector
{
    public static WebApplicationBuilder AddDigitalFilesServices(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions(
            new FilesOptions
            {
                Root = builder.Configuration.Get<string>(AppSettings.FileSystemPath) ?? "/digital_net_storage",
                MaxAvatarSize = builder.Configuration.Get<long>(AppSettings.FileSystemMaxAvatarSize)
            });

        builder.Services.AddScoped<IDocumentService, DocumentService>();
        return builder;
    }
}