using System.Reflection;

namespace Digital.Lib.Net.Core.Application;

public static class ApplicationVersion
{
    private static Assembly GetEntryAssembly() => Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

    /// <summary>
    ///     Get the version of the entry assembly from the "Version" tag in the .csproj file.
    /// </summary>
    public static string GetAssemblyVersion() => GetEntryAssembly().GetName().Version?.ToString() ?? "Not set";
}