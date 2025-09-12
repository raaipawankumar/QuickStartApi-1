using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Data;

namespace QuickStart.Api.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddConfigureDbContext(this IServiceCollection services,
     ConfigurationManager configuration)
    {
        services.AddDbContext<SmartWxReadOnlyContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("smartwx_connection_string");
            options.UseSqlServer(connectionString);
        });
         services.AddDbContext<SmartWxWriteOnlyContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("smartwx_connection_string");
            options.UseSqlServer(connectionString);
        });
        return services;
    }

}
