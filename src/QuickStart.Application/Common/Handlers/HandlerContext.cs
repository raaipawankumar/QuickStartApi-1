using QuickStart.Application.Common.Cache;
using QuickStart.Application.Data;

namespace QuickStart.Application.Common.Handlers;

public class HandlerContext(
    SmartWxReadOnlyContext readOnlyDb,
    SmartWxWriteOnlyContext writeOnlyDb,
     ICache cache) 
 : IHandlerContext
{
 

    public ICache Cache => cache ?? throw new ArgumentNullException($"{nameof(cache)} is null");

    public SmartWxReadOnlyContext DBRead => readOnlyDb;

    public SmartWxWriteOnlyContext DBWrite => writeOnlyDb;
}
