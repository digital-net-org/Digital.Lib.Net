using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Attributes;
using Digital.Lib.Net.Entities.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Entities.Models.ApiKeys;

[Table("ApiKey"), Index(nameof(Key), IsUnique = true)]
public class ApiKey(Guid userId, string? key = null, DateTime? expiredAt = null) : EntityId
{
    public static string Hash(string apiKey)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(apiKey));
        return Convert.ToBase64String(hashBytes);
    }

    [Column("Key"), MaxLength(64), Required, ReadOnly]
    public string Key { get; set; } =
        Hash(key ?? Randomizer.GenerateRandomString(Randomizer.AnyLetterOrNumber, 128));

    [Column("ExpiredAt")]
    public DateTime? ExpiredAt { get; set; } = expiredAt;

    [Column("UserId"), ForeignKey("User"), Required]
    public Guid UserId { get; set; } = userId;

    public virtual User User { get; set; }
}
