using QuickStart.Application.Common.Cache;
using QuickStart.Application.Data;

namespace QuickStart.Application.Common.Handlers
{
    public interface IHandlerContext
    {
        SmartWxReadOnlyContext DBRead { get; }
        SmartWxWriteOnlyContext DBWrite { get; }
        ICache Cache { get; }
   
    }
}
