namespace Safari.Net.Data.Entities;

public class Query
{
    public int Index { get; set; } = QueryUtils.DefaultIndex;
    public int Size { get; set; } = QueryUtils.DefaultSize;
    public string? OrderBy { get; set; }
}