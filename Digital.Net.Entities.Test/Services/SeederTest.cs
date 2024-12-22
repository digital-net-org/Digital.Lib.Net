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

    private readonly TestUser _testUser = new()
    {
        Username = "TestUser",
        Password = "TestPassword",
        Login = "TestLogin",
        Email = "TestEmail@email.com"
    };

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
        var seeded = await _userSeeder.SeedAsync([_testUser]);
        var user = _userRepository.Get(u => u.Username == _testUser.Username);
        Assert.False(seeded.HasError);
        Assert.Single(user);
    }

    [Fact]
    public async Task SeedAsync_SkipExistingEntities()
    {
        await _userFactory.CreateAsync(_testUser);
        var seeded = await _userSeeder.SeedAsync([_testUser]);
        Assert.False(seeded.HasError);
        Assert.Single(_userRepository.Get(u => u.Username == _testUser.Username));
    }
}