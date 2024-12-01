using Digital.Net.Mvc.Test.TestUtilities.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital.Net.Mvc.Test.TestUtilities;

public class TestContext(DbContextOptions<TestContext> options) : DbContext(options)
{
    public DbSet<TestIdEntity> IdUsers { get; set; }
    public DbSet<TestGuidEntity> GuidUsers { get; set; }
}