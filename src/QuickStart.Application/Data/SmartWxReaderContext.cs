using Microsoft.EntityFrameworkCore;

namespace QuickStart.Application.Data;

public class SmartWxReadOnlyContext : SmartWxDbContextBase
{
    public SmartWxReadOnlyContext(DbContextOptions<SmartWxReadOnlyContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = true;
        Database.SetCommandTimeout(30);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new QueryIsolationLevelInterceptor());
    }
    public override int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only. Use WriteDbContext for write operations.");
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This context is read-only. Use WriteDbContext for write operations.");
    }
}
