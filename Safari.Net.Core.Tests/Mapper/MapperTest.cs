using Safari.Net.TestTools;

namespace Safari.Net.Core.Tests.Mapper;

public class MapperTests : UnitTest
{
    [Fact]
    public void Map_ReturnsInstanceOfTargetType_WhenSuitableConstructorFound()
    {
        var source = new TestSourceClass { Value = 42 };
        var result = Models.Mapper.Map<TestSourceClass, TestTargetClass>(source);
        Assert.NotNull(result);
        Assert.IsAssignableFrom<TestTargetClass>(result);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Map_ThrowsInvalidOperationException_WhenNoSuitableConstructorFound()
    {
        var source = new TestSourceClass { Value = 42 };
        Assert.Throws<InvalidOperationException>(
            () => Models.Mapper.Map<TestSourceClass, TestNoSuitableConstructorClass>(source));
    }

    private class TestSourceClass
    {
        public int Value { get; set; }
    }

    private class TestTargetClass(TestSourceClass testSource)
    {
        public int Value { get; } = testSource.Value;
    }

    private class TestNoSuitableConstructorClass(int value)
    {
        public int Value { get; } = value;
    }
}