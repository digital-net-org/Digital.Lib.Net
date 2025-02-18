using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.Avatars;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Entities.Context;

public class DigitalContext(DbContextOptions<DigitalContext> options) : DbContext(options)
{
    public const string Schema = "DigitalNet";

    public DbSet<Avatar> Avatars { get; init; }
    public DbSet<Document> Documents { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<ApiToken> ApiTokens { get; init; }
    public DbSet<ApiKey> ApiKeys { get; init; }
    public DbSet<Event> EventAuthentications { get; init; }

    protected override void OnModelCreating(ModelBuilder builder) => builder.HasDefaultSchema(Schema);
}
