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

    [Fact]
    public void Map_ReturnsListOfTargetType_WhenSuitableConstructorFound()
    {
        var source = new List<TestSourceClass>
        {
            new() { Value = 42 },
            new() { Value = 43 },
            new() { Value = 44 }
        };
        var result = Models.Mapper.Map<TestSourceClass, TestTargetClass>(source);
        Assert.NotNull(result);
        Assert.IsAssignableFrom<List<TestTargetClass>>(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(42, result[0].Value);
        Assert.Equal(43, result[1].Value);
        Assert.Equal(44, result[2].Value);
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