using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Microsoft.Extensions.Logging;

namespace Digital.Lib.Net.Entities.Seeds.Development;

public class DevUserSeed(
    ILogger<DevUserSeed> logger,
    IRepository<ApiKey, DigitalContext> apiKeyRepository,
    IRepository<User, DigitalContext> userRepository
) : Seeder<User, DigitalContext>(logger, userRepository), ISeed
{
    public const string DevUserPassword = "Devpassword123!";
    public override async Task ApplySeed()
    {
        var result = await SeedAsync(Users);
        if (result.HasError())
            throw new Exception(result.Errors.First().Message);

        foreach (var apiKey in result.Value!.Select(BuildUserApiKey))
        {
            await apiKeyRepository.CreateAsync(apiKey);
            await apiKeyRepository.SaveAsync();
        }
    }

    private static List<User> Users =>
    [
        new()
        {
            Username = "admin",
            Login = "admin",
            Password = DevUserPassword,
            Email = "fake-admin@fake.com",
            Role = UserRole.Admin,
            IsActive = true
        },

        new()
        {
            Username = "superAdmin",
            Login = "superAdmin",
            Password = DevUserPassword,
            Email = "fake-super-admin@fake.com",
            Role = UserRole.SuperAdmin,
            IsActive = true
        },

        new()
        {
            Username = "user",
            Login = "user",
            Password = DevUserPassword,
            Email = "fake-user@fake.com",
            Role = UserRole.User,
            IsActive = true
        }
    ];

    private static ApiKey BuildUserApiKey(User user) => new(
        user.Id,
        $"dev_{user.Login.ToLower()}_s12d5fg4h56m56z4ergf561gfj764m4fgsd56fgsj956qierfgd5498746sf8gap9jrp8ez7tazecz079e87u98uo7tyu978az111era98dwckg833574kiumpt"
            [..128]
    );
}