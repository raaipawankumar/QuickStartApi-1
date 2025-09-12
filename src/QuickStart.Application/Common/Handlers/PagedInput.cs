namespace QuickStart.Application.Common.Handlers;

public class PagedInput : IAppQuery
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 5000;
    public static PagedInput Default => new();

}
