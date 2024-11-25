using Digital.Net.Core.Random;
using Microsoft.AspNetCore.Http;

namespace Digital.Net.Core.Extensions.FormFileUtilities;

public static class FormFileInfo
{
    /// <summary>
    ///     Gets the extension of the file.
    /// </summary>
    /// <param name="form">The form file.</param>
    /// <returns>The extension of the file.</returns>
    public static string GetExtension(this IFormFile form) =>
        form.FileName[form.FileName.LastIndexOf('.')..];

    /// <summary>
    ///     Generates a file name from the form file.
    /// </summary>
    /// <param name="form">The form file.</param>
    /// <param name="lenght">The length of the file name.</param>
    /// <returns>A file name generated from the form file.</returns>
    public static string GenerateFileName(this IFormFile form, int? lenght = 64)
    {
        var ext = GetExtension(form);
        return Randomizer.GenerateRandomString(
            Randomizer.CapitalLetters + Randomizer.SmallLetters + Randomizer.Numbers, 64 - ext.Length
        ) + ext;
    }
}