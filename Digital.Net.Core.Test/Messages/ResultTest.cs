using System.Collections;
using System.ComponentModel.DataAnnotations;
using Digital.Net.Core.Extensions.EnumUtilities;
using Digital.Net.Core.Messages;
using Digital.Net.TestTools;

namespace Digital.Net.Core.Test.Messages;

public class ResultTest : UnitTest
{
    [Fact]
    public void AddError_ReturnsResult_WhenException()
    {
        var model = new Result();
        model.AddError(new Exception("Test exception"));
        var error = model.Errors[0];
        Assert.Single((IEnumerable)model.Errors);
        Assert.True(model.HasError);
        Assert.Equal("Test exception", error.Message);
    }

    [Fact]
    public void AddError_ReturnsResult_WhenEnum()
    {
        var model = new Result();
        const TestEnum errorType = TestEnum.Test;
        model.AddError(errorType);
        var error = model.Errors[0];
        Assert.Single((IEnumerable)model.Errors);
        Assert.True(model.HasError);
        Assert.Equal(errorType.GetDisplayName(), error.Message);
    }

    [Fact]
    public void AddInfo_ReturnsResult_WhenEnum()
    {
        var model = new Result();
        const TestEnum infoType = TestEnum.Test;
        model.AddInfo(infoType);
        var info = model.Infos[0];
        Assert.Single(model.Infos);
        Assert.False(model.HasError);
        Assert.Equal(infoType.GetDisplayName(), info.Message);
    }

    [Fact]
    public void Merge_ReturnsResultWithNewErrors()
    {
        var model1 = new Result<string>("Test");
        var model2 = new Result<string>("Toast");

        model1.AddError(new Exception("Error 1"));
        model2.AddError(new Exception("Error 2"));
        var test = model1.Merge(model2);

        Assert.Equal(2, model1.Errors.Count);
        Assert.Equal("Test", model1.Value);
        Assert.Collection(
            model1.Errors,
            error => Assert.Equal("Error 1", error.Message),
            error => Assert.Equal("Error 2", error.Message)
        );
    }

    private enum TestEnum
    {
        [Display(Name = "Test of very simple case")]
        Test,
        Test2
    }
}