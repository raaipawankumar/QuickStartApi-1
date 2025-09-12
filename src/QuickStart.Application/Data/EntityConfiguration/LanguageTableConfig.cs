using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Language;

namespace QuickStart.Application.Data.EntityConfiguration;

public class LanguageTableConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("LanguageDetail", builder => builder.HasTrigger("tr_LanguageDetail"));
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LanguageGuid");
        builder.Property(e => e.Name).HasColumnName("LanguageName");
        builder.Property(e => e.Code).HasColumnName("LanguageCode");
        builder.Property(e => e.Description).HasColumnName("LanguageDescription");
        builder.Property(e => e.IsDefault);
        builder.Property(e => e.Status);

    }


}
