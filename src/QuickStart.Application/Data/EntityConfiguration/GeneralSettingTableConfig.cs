using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.General;

namespace QuickStart.Application.Data.EntityConfiguration;

public class GeneralSettingTableConfig : IEntityTypeConfiguration<GeneralSetting>
{
    public void Configure(EntityTypeBuilder<GeneralSetting> builder)
    {
        builder.ToTable("Settings");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("SettingId");
        builder.Property(e => e.SettingKey);
        builder.Property(e => e.Name);
        builder.Property(e => e.Type);
        builder.Property(e => e.Detail);
        builder.Property(e => e.Value).HasColumnName("ValueText");
        builder.Property(e => e.CompanyId);
        builder.Property(e => e.IsActive);
        builder.Property(e => e.CreatedAt).HasColumnName("RCREATE_");
        builder.Property(e => e.UpdatedAt).HasColumnName("RUPDATE_");




    }
}
