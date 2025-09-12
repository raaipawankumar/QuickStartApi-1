using System.Reflection;
using QuickStart.Api.Common;
using QuickStart.Api.Common.Endpoints;
using QuickStart.Api.Extensions;
using QuickStart.Api.Middlewares;
using QuickStart.Application.Common.Audit;
using Serilog;


try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.SetupSerilog();
    Log.Information("SmartWX admin api started");

    builder.Services
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer()
    .AddConfigureVersioning()
    .AddConfigureSwaggerGen()
    .AddConfigureCors(builder.Configuration)
    .AddConfigureDbContext(builder.Configuration)
    .AddMemoryCache()

    .AddScoped<ExceptionHandlingMiddleware>()
    .AddScoped<AddCorreltationIdMiddleware>()
    .AddSecurityMiddleware(builder.Configuration)
    .AddScoped<IAuditContext, AuditContext>()
    .AddApplicationServices()
    .AddEndpoints(Assembly.GetExecutingAssembly());

    var app = builder.Build();
    app.UseMiddleware<AddCorreltationIdMiddleware>();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseConfigureSwaggerUI();
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
    }
    app.UseMiddleware<SecurityMiddleware>();
    app.UseCors(CORSExtensions.DefaultPolicyName);
    app.UseSerilogRequestLogging();
    app.MapApplicationEndpoints();
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}