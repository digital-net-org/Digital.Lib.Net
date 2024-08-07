using Microsoft.EntityFrameworkCore;
using Safari.Net.Data.Test.TestUtilities.Models;

namespace Safari.Net.Data.Test.TestUtilities;

public class TestContext(DbContextOptions<TestContext> options) : DbContext(options)
{
    public DbSet<FakeUser> Users { get; set; }
    public DbSet<FakeRole> Roles { get; set; }
}