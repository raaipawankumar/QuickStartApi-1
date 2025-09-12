using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Features.General;
using QuickStart.Application.Features.Identity;
using QuickStart.Application.Features.Identity.Role;
using QuickStart.Application.Features.Language;
using QuickStart.Application.Features.Right;
using QuickStart.Application.Features.Role;

namespace QuickStart.Application.Data;

public abstract class SmartWxDbContextBase(DbContextOptions options)
: DbContext(options)
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
    public DbSet<GeneralSetting> GeneralSettings { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleRight> RoleRights { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartWxDbContextBase).Assembly);
    }
}

