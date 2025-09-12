namespace QuickStart.Api.Extensions;

using QuickStart.Application.Common.Cache;
using QuickStart.Application.Common.Handlers;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        // services.AddScoped(typeof(IRepository<>), typeof(DbContextRepository<>));
        
        services.AddScoped<ICache, AppInMemoryCache>();
        services.AddScoped<IHandlerContext, HandlerContext>();
        return services;

    }
}
