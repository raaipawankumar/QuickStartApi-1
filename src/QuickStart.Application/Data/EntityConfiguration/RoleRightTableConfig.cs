using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Right;
using QuickStart.Application.Features.Role;

namespace QuickStart.Application.Data.EntityConfiguration;

public class RoleRightTableConfig : IEntityTypeConfiguration<RoleRight>
{
    public void Configure(EntityTypeBuilder<RoleRight> builder)
    {
        builder.ToTable("RoleRight");
        builder.HasKey(e => e.RoleRightId);
        builder.HasOne<Role>()
        .WithMany()
        .HasForeignKey(e => e.RoleId);
        builder.HasOne<Right>()
        .WithMany()
        .HasForeignKey(e => e.RightId);
    }
}
