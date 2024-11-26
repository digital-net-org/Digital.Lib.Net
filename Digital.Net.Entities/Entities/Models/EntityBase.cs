using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digital.Net.Entities.Attributes;

namespace Digital.Net.Entities.Entities.Models;

/// <summary>
///     Base class for all entities with Timestamps
/// </summary>
public abstract class EntityBase
{
    [Column("created_at")]
    [Required]
    [NoPatch]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")] [NoPatch] public DateTime? UpdatedAt { get; init; }
}