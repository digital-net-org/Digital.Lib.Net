using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digital.Net.Entities.Attributes;

namespace Digital.Net.Entities.Models;

/// <summary>
///     Base class for all entities with Timestamps
/// </summary>
public abstract class EntityBase
{
    [Column("CreatedAt"), Required, ReadOnly]
    public DateTime CreatedAt { get; set; }

    [Column("UpdatedAt"), ReadOnly]
    public DateTime? UpdatedAt { get; init; }
}