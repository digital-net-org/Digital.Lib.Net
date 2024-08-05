using Safari.Net.Core.Extensions.EnumUtilities;
using Safari.Net.TestTools;

namespace Safari.Net.Core.Tests.Extensions.EnumUtilities;

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