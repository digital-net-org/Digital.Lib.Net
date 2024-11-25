using System.ComponentModel.DataAnnotations;
using Digital.Net.Core.Messages;
using Digital.Net.TestTools;

namespace Digital.Net.Core.Tests.Messages;

public class ResultMessageTest : UnitTest
{
    [Fact]
    public void ConstructorTest_ReturnsExceptionValues_WhenCastedWithException()
    {
        var ex = new Exception("Something went wrong");
        var result = new ResultMessage(ex);
        Assert.Equal("Something went wrong", result.Message);
        Assert.Equal("SYSTEM_EXCEPTION", result.Reference);
        Assert.Equal(ex.StackTrace, result.StackTrace);
        Assert.Matches(@"0x[0-9A-F]{8}", result.Code);
    }

    [Fact]
    public void ConstructorTest_ReturnsEnumValues_WhenCastedWithEnum()
    {
        var result = new ResultMessage(ResultMessageEnum.TestMessage);
        Assert.Equal("Test Message", result.Message);
        Assert.Equal("TEST_MESSAGE", result.Reference);
        Assert.Equal("10", result.Code);
        Assert.Null(result.StackTrace);
    }
}

public enum ResultMessageEnum
{
    [Display(Name="Test Message")]
    TestMessage = 10
}