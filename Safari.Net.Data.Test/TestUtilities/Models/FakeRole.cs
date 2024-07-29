using Microsoft.EntityFrameworkCore;
using Safari.Net.Data.Models;

namespace Safari.Net.Data.Test.TestUtilities.Models;

[Index(nameof(Name), IsUnique = true)]
public class FakeRole : EntityWithId
{
    public string Name { get; set; } = string.Empty;
    public ERole Role { get; set; }
}

public enum ERole
{
    SuperAdmin,
    Admin,
    User
}