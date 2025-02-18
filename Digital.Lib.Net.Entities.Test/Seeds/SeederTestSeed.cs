using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Entities.Seeds;
using Microsoft.Extensions.Logging;

namespace Digital.Lib.Net.Entities.Test.Seeds;

public class SeederTestSeed(
    ILogger<SeederTestSeed> logger,
    IRepository<User, DigitalContext> userRepository
) : Seeder<User, DigitalContext>(logger, userRepository), ISeed
{
    public static readonly List<User> Users =
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

    public override async Task ApplySeed() => await SeedAsync(Users);
}