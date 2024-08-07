namespace Safari.Net.Data.Test.TestUtilities.Models;

public class FakeUserModel
{
    public FakeUserModel()
    {
    }

    public FakeUserModel(FakeUser user)
    {
        Username = user.Username;
        Email = user.Email;
        Role = user.Role;
    }

    public string? Username { get; set; }
    public string? Email { get; set; }
    public virtual FakeRole? Role { get; set; }
}