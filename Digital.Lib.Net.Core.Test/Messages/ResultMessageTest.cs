using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.TestTools;

namespace Digital.Lib.Net.Core.Test.Messages;

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
}
