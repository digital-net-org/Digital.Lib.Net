using Digital.Lib.Net.Core.Extensions.HttpUtilities;
using Digital.Lib.Net.TestTools;

namespace Digital.Lib.Net.Core.Test.Extensions.HttpUtilities;

public class QueryStringTests : UnitTest
{
    [Fact]
    public void ToQueryString_ReturnsQueryString_WhenQueryObjectIsNotNull()
    {
        var query = new { TestProperty = "TestValue" };
        var result = query.ToQueryString();
        Assert.Equal("?TestProperty=TestValue", result);
    }
}
