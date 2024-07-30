using Safari.Net.Core.Extensions.EnumUtilities;

namespace Safari.Net.Core.Tests.Extensions.EnumUtilities;

public class EnumValuesTest
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