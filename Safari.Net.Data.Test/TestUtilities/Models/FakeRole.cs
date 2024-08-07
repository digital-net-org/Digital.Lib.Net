using Microsoft.EntityFrameworkCore;
using Safari.Net.Core.Random;
using Safari.Net.Data.Entities.Models;

namespace Safari.Net.Data.Test.TestUtilities.Models;

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