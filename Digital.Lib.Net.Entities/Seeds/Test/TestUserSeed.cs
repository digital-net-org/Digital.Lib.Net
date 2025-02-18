using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.Users;
using Digital.Lib.Net.Entities.Repositories;
using Microsoft.Extensions.Logging;

namespace Digital.Lib.Net.Entities.Seeds.Test;

public class TestUserSeed(
    ILogger<TestUserSeed> logger,
    IRepository<ApiKey, DigitalContext> apiKeyRepository,
    IRepository<User, DigitalContext> userRepository
) : Seeder<User, DigitalContext>(logger, userRepository), ISeed
{
    public const string TestUserPassword = "Testpassword123!";

    public override async Task ApplySeed()
    {
        var result = await SeedAsync(GetUsers());
        if (result.HasError())
            throw new Exception(result.Errors.First().Message);

        foreach (var apiKey in result.Value!.Select(BuildUserApiKey))
        {
            await apiKeyRepository.CreateAsync(apiKey);
            await apiKeyRepository.SaveAsync();
        }
    }

    private static List<User> GetUsers()
    {
        const int userAmount = 15;
        var result = new List<User>();
        for (var i = 0; i < userAmount; i++)
        {
            result.Add(new User
            {
                    Username = Randomizer.GenerateRandomString(Randomizer.AnyLetter, 20),
                    Login = Randomizer.GenerateRandomString(Randomizer.AnyLetterOrNumber, 12),
                    Password = TestUserPassword,
                    Email = Randomizer.GenerateRandomEmail(),
                    Role = i < 1 ? UserRole.SuperAdmin : i < 2 ? UserRole.Admin : UserRole.User,
                    IsActive = i < userAmount - 1
            });
        }
        return result;
    }

    private static ApiKey BuildUserApiKey(User user) => new(
        user.Id,
        $"dev_{user.Login.ToLower()}_s12d5fg4h56m56z4ergf561gfj764m4fgsd56fgsj956qierfgd5498746sf8gap9jrp8ez7tazecz079e87u98uo7tyu978az111era98dwckg833574kiumpt"
            [..128]
    );
}