using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Identity;

namespace QuickStart.Application.Data.EntityConfiguration;

public class UserProfileTableConfig : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserMaster");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ProfileId");
    }
}
