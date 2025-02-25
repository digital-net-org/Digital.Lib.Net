using Digital.Lib.Net.Core.Random;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Digital.Lib.Net.Files.Extensions;

public static class FormFileHelper
{
    public const int DefaultCompressionQuality = 75;

    public static JpegEncoder GetJpegEncoder(int? quality = null) =>
        new() { Quality = quality ?? DefaultCompressionQuality };

    public static string GenerateFileName() =>
        Randomizer.GenerateRandomString(Randomizer.AnyLetterOrNumber, 60);
}