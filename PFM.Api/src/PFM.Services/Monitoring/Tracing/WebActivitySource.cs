using System.Diagnostics;

namespace PFM.Services.Monitoring.Tracing;

public static class ApiActivitySource
{
    public static readonly ActivitySource Source = new ("PFM.Api");

    public static Activity CreateListActivity(string path)
    {
        return Source.CreateActivity($"GET /{path}", ActivityKind.Client);
    }
}