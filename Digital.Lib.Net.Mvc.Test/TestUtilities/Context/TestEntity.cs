using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Attributes;
using Digital.Lib.Net.Entities.Models;

namespace Digital.Lib.Net.Mvc.Test.TestUtilities.Context;

public class TestIdEntity : EntityId
{
    [Column("Username"), Required]
    public string Username { get; set; } = Randomizer.GenerateRandomString();

    [Column("Password"), Required, Secret]
    public string Password { get; set; } = Randomizer.GenerateRandomString();

    [Column("Email")]
    public string Email { get; set; } = Randomizer.GenerateRandomString();
}

public class TestGuidEntity : EntityGuid
{
    [Column("Username"), Required]
    public string Username { get; set; } = Randomizer.GenerateRandomString();

    [Column("Password"), Required, Secret]
    public string Password { get; set; } = Randomizer.GenerateRandomString();

    [Column("Email")]
    public string Email { get; set; } = Randomizer.GenerateRandomString();
}