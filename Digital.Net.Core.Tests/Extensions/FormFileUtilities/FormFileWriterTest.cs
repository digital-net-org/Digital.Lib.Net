using System.Text;
using Digital.Net.Core.Extensions.FormFileUtilities;
using Digital.Net.Core.Random;
using Digital.Net.TestTools;
using Microsoft.AspNetCore.Http;
using Moq;
using Stream = System.IO.Stream;

namespace Digital.Net.Core.Tests.Extensions.FormFileUtilities;

public class FormFileWriterTest : UnitTest
{
    private const string TestPath = "test.txt";
    private readonly Mock<IFormFile> _mockFormFile = new();

    [Fact]
    public void TryWriteFile_Success()
    {
        var stream = Setup();
        _mockFormFile
            .Setup(f => f.CopyTo(It.IsAny<Stream>()))
            .Callback<Stream>(s => stream.CopyTo(s));

        var result = _mockFormFile.Object.TryWriteFile(TestPath);
        Assert.False(result.HasError);
        Assert.True(File.Exists(TestPath));
        File.Delete(TestPath);
    }

    [Fact]
    public void TryWriteFile_Exception()
    {
        _mockFormFile
            .Setup(f => f.CopyTo(It.IsAny<Stream>()))
            .Throws(new Exception("Test exception"));

        var result = _mockFormFile.Object.TryWriteFile(TestPath);
        Assert.True(result.HasError);
    }

    [Fact]
    public async Task TryWriteFileAsync_Success()
    {
        var stream = Setup();
        _mockFormFile
            .Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
            .Returns<Stream, CancellationToken>((s, t) => stream.CopyToAsync(s, t));

        var result = await _mockFormFile.Object.TryWriteFileAsync(TestPath);
        Assert.False(result.HasError);
        Assert.True(File.Exists(TestPath));
        File.Delete(TestPath);
    }

    [Fact]
    public async Task TryWriteFileAsync_Exception()
    {
        _mockFormFile
            .Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
            .ThrowsAsync(new Exception("Test exception"));

        var result = await _mockFormFile.Object.TryWriteFileAsync(TestPath);
        Assert.True(result.HasError);
    }

    private static MemoryStream Setup()
    {
        var contentBytes = Encoding.UTF8.GetBytes(Randomizer.GenerateRandomString());
        var stream = new MemoryStream(contentBytes);
        return stream;
    }
}