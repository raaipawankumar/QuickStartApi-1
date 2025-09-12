using Microsoft.EntityFrameworkCore;

namespace QuickStart.Application.Data;

public class SmartWxWriteOnlyContext : SmartWxDbContextBase
{
    public SmartWxWriteOnlyContext(DbContextOptions<SmartWxWriteOnlyContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        ChangeTracker.AutoDetectChangesEnabled = true;
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