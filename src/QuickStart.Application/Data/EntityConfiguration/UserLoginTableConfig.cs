using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickStart.Application.Features.Identity;

namespace QuickStart.Application.Data.EntityConfiguration;

public class UserLoginTableConfig: IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable("UserLogin");
        builder.HasKey("ProfileId");
        
    }
}
