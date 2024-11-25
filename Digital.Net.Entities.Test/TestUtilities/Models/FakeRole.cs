using Digital.Net.Core.Random;
using Digital.Net.Entities.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital.Net.Entities.Test.TestUtilities.Models;

[Index(nameof(Name), IsUnique = true)]
public class FakeRole : EntityWithId
{
    public string Name { get; set; } = Randomizer.GenerateRandomString();
    public ERole Role { get; set; }
}

public enum ERole
{
    SuperAdmin,
    Admin,
    User
}