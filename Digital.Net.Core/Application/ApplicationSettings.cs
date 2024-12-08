using Digital.Net.Core.Environment;
using Microsoft.Extensions.Configuration;

namespace Digital.Net.Core.Application;

public static class ApplicationSettings
{
    /// <summary>
    ///     Add application settings to the configuration builder from the provided project path.
    /// </summary>
    /// <param name="builder">The configuration builder.</param>
    /// <param name="projectName">The name of the project.</param>
    public static IConfigurationBuilder AddProjectSettings(this IConfigurationBuilder builder, string projectName) =>
        builder
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", projectName))
            .AddAppSettings();

    /// <summary>
    ///     Add the following to the configuration builder:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>appsettings.json</description>
    ///         </item>
    ///         <item>
    ///             <description>appsettings.{environment}.json</description>
    ///         </item>
    ///         <item>
    ///             <description>environment variables</description>
    ///         </item>
    ///     </list>
    /// </summary>
    /// <param name="builder">The configuration builder.</param>
    /// <returns>The updated configuration builder.</returns>
    public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder builder) =>
        builder
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{AspNetEnv.Get}.json", true, true)
            .AddEnvironmentVariables();
}