using Digital.Lib.Net.Core.Extensions.EnumUtilities;
using Digital.Lib.Net.TestTools;

namespace Digital.Lib.Net.Core.Test.Extensions.EnumUtilities;

public class EnumValuesTest : UnitTest
{
    private enum ETest
    {
        Test,
        Test2
    }

    [Fact]
    public void GetEnumValues_ReturnsEnumValues() =>
        Assert.Equal([ETest.Test, ETest.Test2], EnumDisplay.GetEnumValues<ETest>());
}