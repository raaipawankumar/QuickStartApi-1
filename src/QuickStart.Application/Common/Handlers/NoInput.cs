namespace QuickStart.Application.Common.Handlers;

public class NoInput : IAppQuery
{
    private NoInput() { }
    public static NoInput Instance => new();
}
