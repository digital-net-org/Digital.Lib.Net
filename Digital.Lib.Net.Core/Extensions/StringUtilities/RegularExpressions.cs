using System.Text.RegularExpressions;

namespace Digital.Lib.Net.Core.Extensions.StringUtilities;

/// <summary>
///     Collection of regular expressions for string manipulation.
/// </summary>
public static partial class RegularExpressions
{
    [GeneratedRegex("(?<!^)([A-Z])")]
    public static partial Regex PascalCase();

    [GeneratedRegex("(?<!^)([.])")]
    public static partial Regex ObjectName();
}