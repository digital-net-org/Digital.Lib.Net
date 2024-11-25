using Digital.Net.Entities.Entities.Models;

namespace Digital.Net.Entities.Entities;

public class Query : EntityBase
{
    public int Index { get; set; } = QueryUtils.DefaultIndex;
    public int Size { get; set; } = QueryUtils.DefaultSize;
    public string? OrderBy { get; set; }

    public new DateTime? CreatedAt { get; set; }
}