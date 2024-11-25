using Digital.Net.Entities.Entities;
using Digital.Net.TestTools;

namespace Digital.Net.Entities.Test.Entities;

public class QueryUtilsTest : UnitTest
{
    [Fact]
    public void ValidateParameters_SetsDefaultIndex_WhenIndexIsLessThanOne()
    {
        var query = new Query { Index = -1, Size = 1 };
        query.ValidateParameters();
        Assert.Equal(QueryUtils.DefaultIndex, query.Index);
    }

    [Fact]
    public void ValidateParameters_SetsDefaultSize_WhenSizeIsLessThanOne()
    {
        var query = new Query { Index = 1, Size = -1 };
        query.ValidateParameters();
        Assert.Equal(QueryUtils.DefaultSize, query.Size);
    }
}