using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Mvc.Test.TestUtilities.Context;

public class MvcTestContext(DbContextOptions<MvcTestContext> options) : DbContext(options)
{
    public DbSet<TestIdEntity> IdUsers { get; set; }
    public DbSet<TestGuidEntity> GuidUsers { get; set; }
}