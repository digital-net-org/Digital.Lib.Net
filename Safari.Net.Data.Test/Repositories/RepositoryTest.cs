using Safari.Net.Data.Repositories;
using Safari.Net.Data.Test.TestUtilities;
using Safari.Net.Data.Test.TestUtilities.Models;
using Safari.Net.TestTools;
using Safari.Net.TestTools.Data;
using Safari.Net.TestTools.Data.Factories;

namespace Safari.Net.Data.Test.Repositories;

public class RepositoryTest : UnitTest
{
    private readonly DataFactory<FakeUser> _userFactory;
    private readonly DataFactory<FakeRole> _roleFactory;
    private readonly Repository<FakeUser, TestContext> _userRepository;
    private readonly Repository<FakeRole, TestContext> _roleRepository;

    public RepositoryTest()
    {
        var context = new SqliteMemoryDb<TestContext>().Context;
        _userRepository = new Repository<FakeUser, TestContext>(context);
        _roleRepository = new Repository<FakeRole, TestContext>(context);
        _userFactory = new DataFactory<FakeUser>(_userRepository);
        _roleFactory = new DataFactory<FakeRole>(_roleRepository);
    }

    [Fact]
    public void GetById_ReturnsUser_WhenIdIsGuidAndUserExists()
    {
        var user = _userFactory.Create();
        var userById = _userRepository.GetById(user.Id);
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Id);
    }

    [Fact]
    public async void GetByIdAsync_ReturnsUser_WhenIdIsGuidAndUserExists()
    {
        var user = await _userFactory.CreateAsync();
        var userById = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Id);
    }

    [Fact]
    public void GetById_ReturnsUser_WhenIdIsIntAndUserExists()
    {
        var role = _roleFactory.Create();
        var roleById = _roleRepository.GetById(role.Id);
        Assert.NotNull(roleById);
        Assert.Equal(role.Id, roleById.Id);
    }

    [Fact]
    public async void GetByIdAsync_ReturnsUser_WhenIdIsIntAndUserExists()
    {
        var role = await _roleFactory.CreateAsync();
        var roleById = await _roleRepository.GetByIdAsync(role.Id);
        Assert.NotNull(roleById);
        Assert.Equal(role.Id, roleById.Id);
    }

    [Fact]
    public void Create_ShouldCreateUser()
    {
        var user = new FakeUser { Username = "Garfield" };
        _userRepository.Create(user);
        _userRepository.Save();
        var created = _userRepository.Get(u => u.Username == user.Username).FirstOrDefault();
        Assert.NotNull(created);
        Assert.Equal(user.Username, created.Username);
    }

    [Fact]
    public async void CreateAsync_ShouldCreateUser()
    {
        var user = await _userFactory.CreateAsync();
        var createdUser = _userRepository.Get(u => u.Username == user.Username).FirstOrDefault();
        Assert.NotNull(createdUser);
        Assert.Equal(user.Username, createdUser.Username);
        Assert.True(createdUser.CreatedAt <= DateTime.UtcNow);
    }


    [Fact]
    public void Update_ShouldUpdateEntity()
    {
        var user = _userFactory.Create();
        user.Username = "John Arbuckle";
        _userRepository.Update(user);
        _userRepository.Save();
        var updatedUser = _userRepository.Get(u => u.Username == user.Username).FirstOrDefault()!;
        Assert.Equal(user.Username, updatedUser.Username);
        Assert.True(updatedUser.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Delete_ShouldDeleteUser()
    {
        var user = _userFactory.Create();
        _userRepository.Delete(user);
        _userRepository.Save();
        var deleted = _userRepository.Get(u => u.Username == user.Username).FirstOrDefault();
        Assert.True(deleted is null);
    }
}