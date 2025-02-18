using System.Text.RegularExpressions;
using Digital.Lib.Net.Core.Extensions.StringUtilities;

namespace Digital.Lib.Net.Authentication.Options.Config;

public partial class PasswordConfig
{
    /// <summary>
    ///     The regex to validate the password.
    ///     Default to "^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\\da-zA-Z]).{12,128}$"
    ///     Which means: At least one lowercase, one uppercase, one digit, one special character and between 12 and 128
    ///     characters.
    /// </summary>
    public Regex PasswordRegex { get; set; } = new(RegularExpressions.PasswordPattern);
}