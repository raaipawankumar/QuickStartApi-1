using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Identity;

namespace QuickStart.Application.Data.EntityConfiguration;

public class UserLoginHistoryTableConfig : IEntityTypeConfiguration<UserLoginHistory>
{
    public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
    {
        builder.ToTable("UserLoginHistory");
        builder.HasKey(e => e.HistoryId);
    }
}
