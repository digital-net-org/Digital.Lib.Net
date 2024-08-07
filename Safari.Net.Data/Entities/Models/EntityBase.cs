using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safari.Net.Data.Entities.Models;

/// <summary>
///     Base class for all entities with Timestamps
/// </summary>
public abstract class EntityBase
{
    [Column("created_at")] [Required] public DateTime CreatedAt { get; set; }

    [Column("updated_at")] public DateTime? UpdatedAt { get; init; }
}