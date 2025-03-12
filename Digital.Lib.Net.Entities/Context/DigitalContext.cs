using Digital.Lib.Net.Entities.Models.ApiKeys;
using Digital.Lib.Net.Entities.Models.ApiTokens;
using Digital.Lib.Net.Entities.Models.ApplicationOptions;
using Digital.Lib.Net.Entities.Models.Avatars;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Models.Events;
using Digital.Lib.Net.Entities.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Entities.Context;

public class DigitalContext(DbContextOptions<DigitalContext> options) : DbContext(options)
{
    public const string Schema = "digital_core";

    public DbSet<ApiKey> ApiKeys { get; init; }
    public DbSet<ApiToken> ApiTokens { get; init; }
    public DbSet<ApplicationOption> ApplicationOptions { get; init; }
    public DbSet<Avatar> Avatars { get; init; }
    public DbSet<Document> Documents { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Schema);

        builder
            .Entity<User>()
            .HasMany<ApiKey>()
            .WithOne(ak => ak.User)
            .HasForeignKey(ak => ak.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Entity<User>()
            .HasMany<ApiToken>()
            .WithOne(at => at.User)
            .HasForeignKey(at => at.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Entity<User>()
            .HasMany<Document>()
            .WithOne(d => d.Uploader)
            .HasForeignKey(d => d.UploaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Entity<User>()
            .HasMany<Event>()
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Entity<User>()
            .HasOne(u => u.Avatar)
            .WithMany()
            .HasForeignKey(u => u.AvatarId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
