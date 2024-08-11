using Safari.Net.Core.Extensions.HttpUtilities;
using Safari.Net.TestTools;

namespace Safari.Net.Core.Tests.Extensions.HttpUtilities;

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
