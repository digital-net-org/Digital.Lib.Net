using Digital.Net.Entities.Repositories;
using Digital.Net.Entities.Services;
using Digital.Net.TestTools;
using Digital.Net.TestTools.Data;
using Digital.Net.TestTools.Data.Factories;
using InternalTestProgram;
using InternalTestProgram.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Digital.Net.Entities.Test.Services;

public class SeederTest : UnitTest
{
    private readonly DataFactory<TestUser> _userFactory;
    private readonly Repository<TestUser> _userRepository;
    private readonly ISeeder<TestUser> _userSeeder;

    private readonly List<TestUser> _testUsers =
    [
        new()
        {
            Username = "TestUser",
            Password = "TestPassword",
            Login = "TestLogin",
            Email = "TestEmail@email.com"
        },
        new()
        {
            Username = "TestUser2",
            Password = "TestPassword2",
            Login = "TestLogin2",
            Email = "TestEmail2@email.com"
        }
    ];

    public SeederTest()
    {
        var context = new SqliteMemoryDb<TestContext>().Context;
        _userRepository = new Repository<TestUser>(context);
        _userFactory = new DataFactory<TestUser>(_userRepository);
        _userSeeder = new Seeder<TestUser>(
            new Mock<ILogger<Seeder<TestUser>>>().Object,
            _userRepository
        );
    }

    [Fact]
    public async Task SeedAsync_AddEntitiesToDatabase()
    {
        var result = await _userSeeder.SeedAsync(_testUsers);
        var users = _userRepository.Get(x => true);
        Assert.False(result.HasError());
        Assert.True(users.Count() == 2);
    }

    [Fact]
    public async Task SeedAsync_SkipExistingEntities()
    {
        await _userFactory.CreateAsync(_testUsers[0]);
        var result = await _userSeeder.SeedAsync([_testUsers[0]]);
        Assert.False(result.HasError());
        Assert.Single(_userRepository.Get(u => u.Username == _testUsers[0].Username));
    }

    [Fact]
    public async Task SeedAsync_ReturnsSeededEntities()
    {
        await _userFactory.CreateAsync(_testUsers[0]);
        var result = await _userSeeder.SeedAsync(_testUsers);
        var user = result.Value!.Find(u => u.Username == _testUsers[1].Username);
        Assert.False(result.HasError());
        Assert.Single(result.Value!);
        Assert.True(user is not null && user.Id != Guid.Empty);
    }
}