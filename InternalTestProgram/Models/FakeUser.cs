using System.ComponentModel.DataAnnotations.Schema;
using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Models;

namespace InternalTestProgram.Models;

[Table("FakeUser")]
public class FakeUser : EntityGuid, IApiUser
{
    public string Password { get; set; } = Randomizer.GenerateRandomString();
    public string Login { get; } = Randomizer.GenerateRandomString();
    public bool IsActive { get; } = true;
}