namespace Digital.Lib.Net.Core.Extensions.StringUtilities;

public static class StringMatcher
{
    public static bool IsJsonWebToken(this string token) =>
        !string.IsNullOrWhiteSpace(token) && token.Split('.').Length == 3;
}