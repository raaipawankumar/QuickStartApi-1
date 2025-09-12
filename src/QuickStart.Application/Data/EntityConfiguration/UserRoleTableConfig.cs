using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Identity;
using QuickStart.Application.Features.Identity.Role;
using QuickStart.Application.Features.Role;

namespace QuickStart.Application.Data.EntityConfiguration;

public class UserRoleTableConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(e => e.UserRoleId);
        builder.HasOne<UserProfile>()
        .WithMany()
        .HasForeignKey(e => e.ProfileId);
        builder.HasOne<Role>()
        .WithMany()
        .HasForeignKey(e => e.RoleId);
    }
}
