using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Role;

namespace QuickStart.Application.Data.EntityConfiguration;

public class RoleTableConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("RoleId");
        builder.Property(e => e.Code).HasColumnName("RoleCode");
        builder.Property(e => e.Name).HasColumnName("Role");
    }
}
