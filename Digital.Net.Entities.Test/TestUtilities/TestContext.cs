using Digital.Net.Entities.Test.TestUtilities.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital.Net.Entities.Test.TestUtilities;

public class TestContext(DbContextOptions<TestContext> options) : DbContext(options)
{
    public DbSet<FakeUser> Users { get; set; }
    public DbSet<FakeRole> Roles { get; set; }
}