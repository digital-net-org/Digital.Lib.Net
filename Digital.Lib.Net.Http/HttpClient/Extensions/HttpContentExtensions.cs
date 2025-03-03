using Digital.Lib.Net.Http.Serialization;

namespace Digital.Lib.Net.Http.HttpClient.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T> ReadContentAsync<T>(this HttpContent content)
    {
        var value = await content.ReadAsStringAsync();
        return DigitalSerializer.Deserialize<T>(value);
    }
}