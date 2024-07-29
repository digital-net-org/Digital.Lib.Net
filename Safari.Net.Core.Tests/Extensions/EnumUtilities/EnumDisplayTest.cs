using System.ComponentModel.DataAnnotations;
using Safari.Net.Core.Extensions.EnumUtilities;
using Safari.Net.TestTools;

namespace Safari.Net.Core.Tests.Extensions.EnumUtilities;

public class EnumDisplayTest : UnitTest
{
    [Fact]
    public void GetDisplayName_ReturnsEnumDisplayName_WhenAttributeIsSet() =>
        Assert.Equal("Test of very simple case", ETest.Test.GetDisplayName());

    [Fact]
    public void GetDisplayName_ReturnsEmptyString_WhenAttributeIsNotSet() =>
        Assert.Equal(string.Empty, ETest.Test2.GetDisplayName());

    [Fact]
    public void GetEnumValues_ReturnsEnumValues() =>
        Assert.Equal([ETest.Test, ETest.Test2], EnumDisplay.GetEnumValues<ETest>());

    [Fact]
    public void GetEnumDisplayNames_ReturnsEnumDisplayNames() =>
        Assert.Equal(
            ["Test of very simple case", string.Empty],
            EnumDisplay.GetEnumDisplayNames<ETest>()
        );

    private enum ETest
    {
        [Display(Name = "Test of very simple case")]
        Test,
        Test2
    }
}
