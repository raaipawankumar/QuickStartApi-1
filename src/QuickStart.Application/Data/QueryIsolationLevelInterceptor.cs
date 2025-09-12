using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace QuickStart.Application.Data;

public class QueryIsolationLevelInterceptor : DbTransactionInterceptor
{
    public override InterceptionResult<DbTransaction> TransactionStarting(
        DbConnection connection,
        TransactionStartingEventData eventData,
        InterceptionResult<DbTransaction> result)
    {
        return InterceptionResult<DbTransaction>.SuppressWithResult(
            connection.BeginTransaction(IsolationLevel.ReadUncommitted));
    }
}
