using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Right;

namespace QuickStart.Application.Data.EntityConfiguration;

public class RightTableConfig : IEntityTypeConfiguration<Right>
{
    public void Configure(EntityTypeBuilder<Right> builder)
    {
        builder.ToTable("Rights");
        builder.HasKey(e => e.RightId);
        builder.Property(e => e.Name).HasColumnName("Right");
    }
}
