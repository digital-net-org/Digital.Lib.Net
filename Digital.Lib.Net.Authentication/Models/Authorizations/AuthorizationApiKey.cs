using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digital.Lib.Net.Authentication.Services.Security;
using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Attributes;
using Digital.Lib.Net.Entities.Models;

namespace Digital.Lib.Net.Authentication.Models.Authorizations;

public abstract class AuthorizationApiKey : EntityId, IAuthorizationKey
{
    [Column("Key"), MaxLength(64), Required, ReadOnly]
    public string Key { get; set; } = HashService.HashApiKey(
        Randomizer.GenerateRandomString(Randomizer.AnyLetterOrNumber, 128)
    );

    [Column("ExpiredAt")]
    public DateTime? ExpiredAt { get; set; }

    [Column("ApiUserId"), Required]
    public Guid ApiUserId { get; set; }
}