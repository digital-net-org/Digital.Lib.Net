using Digital.Net.Core.Random;
using Digital.Net.Entities.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital.Net.Entities.Test.TestUtilities.Models;

[Index(nameof(Username), nameof(Email), IsUnique = true)]
public class FakeUser : EntityWithGuid
{
    public string Username { get; set; } = Randomizer.GenerateRandomString();
    public string Password { get; set; } = Randomizer.GenerateRandomString();
    public string Email { get; set; } = Randomizer.GenerateRandomString();
    public virtual FakeRole Role { get; set; } = new();
}