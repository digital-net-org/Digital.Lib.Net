using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digital.Lib.Net.Entities.Attributes;

namespace Digital.Lib.Net.Entities.Models;

/// <summary>
///     Base class for all entities with Timestamps
/// </summary>
public abstract class Entity : IEntity
{
    [Column("CreatedAt"), Required, ReadOnly]
    public DateTime CreatedAt { get; set; }

    [Column("UpdatedAt"), ReadOnly]
    public DateTime? UpdatedAt { get; set; }
}
