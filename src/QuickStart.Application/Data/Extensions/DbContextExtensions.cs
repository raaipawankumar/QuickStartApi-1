using Microsoft.EntityFrameworkCore;

namespace QuickStart.Application.Data.Extensions;

public static class DbContextExtensions
{
    public static string GetTableName<TType>(this DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        return dbContext.Model.FindEntityType(typeof(TType))?.GetTableName()
            ?? $"{typeof(Type).Name}_Missing";
    }

}
