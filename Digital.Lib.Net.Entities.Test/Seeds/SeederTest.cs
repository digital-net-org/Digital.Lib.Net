using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.TestTools;
using Digital.Lib.Net.TestTools.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace Digital.Lib.Net.Entities.Test.Seeds;

public class SeederTest : UnitTest
{
    private readonly SeederTestSeed _userSeeder;
    private readonly Repository<User, DigitalContext> _userRepository;

    public SeederTest()
    {
        var context = new SqliteMemoryDb<DigitalContext>().Context;
        _userRepository = new Repository<User, DigitalContext>(context);
        _userSeeder = new SeederTestSeed(
            new Mock<ILogger<SeederTestSeed>>().Object,
            _userRepository
        );
    }

    [Fact]
    public async Task SeedAsync_AddEntitiesToDatabase()
    {
        var result = await _userSeeder.SeedAsync(SeederTestSeed.Users);
        var users = _userRepository.Get(x => true);
        Assert.False(result.HasError());
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task SeedAsync_SkipExistingEntities()
    {
        await _userRepository.CreateAsync(SeederTestSeed.Users[0]);
        await _userRepository.SaveAsync();

        var result = await _userSeeder.SeedAsync([SeederTestSeed.Users[0]]);
        Assert.False(result.HasError());
        Assert.Single(_userRepository.Get(u => u.Username == SeederTestSeed.Users[0].Username));
    }

    [Fact]
    public async Task SeedAsync_ReturnsSeededEntities()
    {
        await _userRepository.CreateAsync(SeederTestSeed.Users[0]);
        await _userRepository.SaveAsync();

        var result = await _userSeeder.SeedAsync(SeederTestSeed.Users);
        var user = result.Value!.Find(u => u.Username == SeederTestSeed.Users[1].Username);
        Assert.False(result.HasError());
        Assert.Single(result.Value!);
        Assert.True(user is not null && user.Id != Guid.Empty);
    }
}