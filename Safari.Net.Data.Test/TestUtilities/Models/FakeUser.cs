using Microsoft.EntityFrameworkCore;
using Safari.Net.Data.Entities.Models;

namespace Safari.Net.Data.Test.TestUtilities.Models;

[Index(nameof(Username), nameof(Email), IsUnique = true)]
public class FakeUser : EntityWithGuid
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public virtual FakeRole Role { get; set; } = new();
}