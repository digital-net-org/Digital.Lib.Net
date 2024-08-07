using Microsoft.EntityFrameworkCore;
using Safari.Net.Core.Random;
using Safari.Net.Data.Entities.Models;

namespace Safari.Net.Data.Test.TestUtilities.Models;

[Index(nameof(Username), nameof(Email), IsUnique = true)]
public class FakeUser : EntityWithGuid
{
    public string Username { get; set; } = Randomizer.GenerateRandomString();
    public string Password { get; set; } = Randomizer.GenerateRandomString();
    public string Email { get; set; } = Randomizer.GenerateRandomString();
    public virtual FakeRole Role { get; set; } = new();
}