using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Safari.Net.Core.Extensions.EnumUtilities;

public static class EnumDisplay
{
    public static string GetDisplayName(this Enum enumValue) =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.Name ?? string.Empty;

    public static IEnumerable<string> GetEnumDisplayNames<T>()
        where T : Enum => GetEnumValues<T>().Select(e => e.GetDisplayName());

    public static IEnumerable<T> GetEnumValues<T>()
        where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();
}
