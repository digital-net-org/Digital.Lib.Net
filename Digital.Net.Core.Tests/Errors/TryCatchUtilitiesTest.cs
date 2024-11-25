using Digital.Net.Core.Errors;
using Digital.Net.TestTools;

namespace Digital.Net.Core.Tests.Errors;

public class TryCatchUtilitiesTests : UnitTest
{
    [Fact]
    public void TryOrNull_ReturnsResult_WhenFunctionExecutesSuccessfully()
    {
        var result = TryCatchUtilities.TryOrNull(() => 42);
        Assert.Equal(42, result);
    }

    [Fact]
    public void TryOrNull_ReturnsNull_WhenFunctionThrowsException()
    {
        var result = TryCatchUtilities.TryOrNull<string>(
            () => throw new InvalidOperationException());
        Assert.Null(result);
    }

    [Fact]
    public async Task TryOrNullAsync_ReturnsResult_WhenFunctionExecutesSuccessfully()
    {
        var result = await TryCatchUtilities.TryOrNullAsync(async () => await Task.FromResult(42));
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task TryOrNullAsync_ReturnsNull_WhenFunctionThrowsException()
    {
        var result = await TryCatchUtilities.TryOrNullAsync<string>(
            async () => throw new InvalidOperationException());
        Assert.Null(result);
    }
}