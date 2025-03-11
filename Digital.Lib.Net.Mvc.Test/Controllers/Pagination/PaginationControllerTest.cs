using Digital.Lib.Net.Core.Interval;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Controllers.Pagination;
using Digital.Lib.Net.Mvc.Test.TestUtilities.Context;
using Digital.Lib.Net.Mvc.Test.TestUtilities.Controllers;
using Digital.Lib.Net.TestTools;
using Digital.Lib.Net.TestTools.Data;
using Digital.Lib.Net.TestTools.Data.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Digital.Lib.Net.Mvc.Test.Controllers.Pagination;

public class PaginationControllerTest : UnitTest, IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PaginationControllerWithId _paginationController;
    private readonly DataFactory<TestIdEntity, MvcTestContext> _testEntityFactory;
    private readonly Repository<TestIdEntity, MvcTestContext> _testEntityRepository;

    public PaginationControllerTest()
    {
        _connection = SqliteInMemoryHelper.GetConnection();
        var context = _connection.CreateContext<MvcTestContext>();
        _testEntityRepository = new Repository<TestIdEntity, MvcTestContext>(context);
        _testEntityFactory = new DataFactory<TestIdEntity, MvcTestContext>(context);
        _paginationController = new PaginationControllerWithId(_testEntityRepository);
    }

    private QueryResult<TestIdEntityDto> Test(TestIdEntityQuery query)
    {
        var actionResult = _paginationController.Get(query).Result as OkObjectResult;
        return actionResult?.Value as QueryResult<TestIdEntityDto> ?? new QueryResult<TestIdEntityDto>();
    }

    [Fact]
    public void Get_ReturnsMappedModelWithCorrectPagination_WhenQueryIsValid()
    {
        const int total = 10;
        const int index = 1;
        const int size = 5;
        _testEntityFactory.CreateMany(total);
        var result = Test(new TestIdEntityQuery { Index = index, Size = size });

        Assert.Equal(total, result.Total);
        Assert.Equal(size, result.Size);
        Assert.Equal(index, result.Index);
        Assert.Equal(size, result.Count);
    }

    [Fact]
    public void Get_ReturnsCorrectItems_WhenFilteredWithMutationDates()
    {
        for (var i = 1; i < 3; i++)
            _testEntityFactory.Create(new TestIdEntity { CreatedAt = DateTime.UtcNow.AddDays(-i + 1) });

        var result = Test(new TestIdEntityQuery { CreatedAt = DateTime.UtcNow.AddDays(-1) });
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Get_ReturnsCorrectItems_WhenFilteredWithMutationDateRanges()
    {
        var now = DateTime.UtcNow;
        var users = _testEntityFactory.CreateMany(5);
        foreach (var user in users)
        {
            var i = users.IndexOf(user);
            user.CreatedAt = now.AddDays(i);
            _testEntityRepository.Save();
        }

        var result = Test(new TestIdEntityQuery { CreatedIn = new DateRange { From = now, To = now.AddDays(2) } });
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Get_ReturnsCorrectItems_WhenIndexInSecondPage()
    {
        const int total = 10;
        const int index = 2;
        const int size = 5;
        var users = _testEntityFactory.CreateMany(total);
        foreach (var user in users)
        {
            var i = users.IndexOf(user) + 1;
            user.Username = $"User{i}";
            user.CreatedAt = DateTime.UtcNow.AddDays(-total + i);
            _testEntityRepository.Save();
        }

        var result = Test(new TestIdEntityQuery { Index = index, Size = size, OrderBy = "CreatedAt" });
        Assert.Equal("User6", result.Value.First().Username);
    }

    [Fact]
    public void Get_ReturnsError_WhenInvalidOrder()
    {
        const int total = 10;
        _testEntityFactory.CreateMany(total);
        var result = Test(new TestIdEntityQuery { OrderBy = "Lol" });
        Assert.True(result.HasError());
        Assert.NotEmpty(result.Errors);
    }

    public void Dispose() => _connection.Dispose();
}