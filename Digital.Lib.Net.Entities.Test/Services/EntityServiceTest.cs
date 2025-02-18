using System.Linq.Expressions;
using Digital.Lib.Net.Core.Exceptions;
using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Exceptions;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Entities.Services;
using Digital.Lib.Net.TestTools;
using Digital.Lib.Net.TestTools.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace Digital.Lib.Net.Entities.Test.Services;

public class EntityServiceTest : UnitTest
{
    private static User GetTestUser() => new()
    {
            Username = Randomizer.GenerateRandomString(),
            Login = Randomizer.GenerateRandomString(),
            Password = Randomizer.GenerateRandomString(),
            Email = Randomizer.GenerateRandomEmail()
    };

    private static JsonPatchDocument<User> CreateUserPatch<T>(Expression<Func<User, T>> path, T value)
    {
        var patch = new JsonPatchDocument<User>();
        patch.Replace(path, value);
        return patch;
    }

    private readonly Repository<User, DigitalContext> _userRepository;
    private readonly IEntityService<User, DigitalContext> _userService;

    public EntityServiceTest()
    {
        var context = new SqliteMemoryDb<DigitalContext>().Context;
        _userRepository = new Repository<User, DigitalContext>(context);
        _userService = new EntityService<User, DigitalContext>(_userRepository);
    }

    [Fact]
    public void GetSchema_ReturnsCorrectSchema_WhenEntityHasProperties() =>
        Assert.Equal("Username", _userService.GetSchema()[0].Name);

    [Fact]
    public async Task Patch_ReturnsMappedModel_WhenQueryIsValid()
    {
        var user = await _userRepository.CreateAndSaveAsync(GetTestUser());
        var patch = CreateUserPatch(u => u.Username, "NewUsername");

        var result = await _userService.Patch(patch, user.Id);
        var updatedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(result);
        Assert.Equal("NewUsername", updatedUser?.Username);
    }

    [Fact]
    public async Task Patch_ReturnsError_WhenEntityNotFound()
    {
        var result = await _userService.Patch(
            CreateUserPatch(u => u.Username, "NewUsername"),
            Guid.NewGuid()
        );
        Assert.True(result.HasError<ResourceNotFoundException>());
    }

    [Fact]
    public async Task Patch_ReturnsError_WhenInvalidRegex()
    {
        var user = await _userRepository.CreateAndSaveAsync(GetTestUser());
        var patch = CreateUserPatch(u => u.Username, "to");
        var result = await _userService.Patch(patch, user.Id);
        var updatedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.True(result.HasError<EntityValidationException>());
        Assert.NotEqual("", updatedUser?.Username);
    }
    
    [Fact]
    public async Task Patch_ReturnsError_WhenUniqueConstraint()
    {
        var user = await _userRepository.CreateAndSaveAsync(GetTestUser());
        var user2 = await _userRepository.CreateAndSaveAsync(GetTestUser());
        var patch = CreateUserPatch(u => u.Username, user2.Username);
        var result = await _userService.Patch(patch, user.Id);
        var updatedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.True(result.HasError());
        Assert.NotEqual(user2.Username, updatedUser?.Username);
    }
    
    [Fact]
    public async Task Patch_ReturnsError_WhenPatchingReadOnlyField()
    {
        var user = await _userRepository.CreateAndSaveAsync(GetTestUser());
        var patch = CreateUserPatch(u => u.Password, "testValue");
        var result = await _userService.Patch(patch, user.Id);
        Assert.True(result.HasError());
    }

    [Fact]
    public async Task Create_ReturnsSuccess_WhenEntityIsValid()
    {
        // var user = await _userRepository.CreateAndSaveAsync(GetTestUser());
        // var createdUser = await _userRepository.GetByIdAsync(user.Id);
        // Assert.False(result.HasError());
        // Assert.NotNull(createdUser);
        // Assert.Equal("NewUser", createdUser.Username);
    }

    [Fact]
    public async Task Delete_ReturnsSuccess_WhenEntityExists()
    {
        var user = await _userRepository.CreateAndSaveAsync(GetTestUser());
        var result = await _userService.Delete(user.Id);
        var deletedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.False(result.HasError());
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenEntityDoesNotExist()
    {
        var result = await _userService.Delete(Guid.NewGuid());
        Assert.True(result.HasError());
    }
}