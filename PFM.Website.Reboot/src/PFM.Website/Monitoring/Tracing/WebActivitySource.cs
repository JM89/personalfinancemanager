using System.Diagnostics;

namespace PFM.Website.Monitoring.Tracing;

public static class WebActivitySource
{
    public static readonly ActivitySource Source = new ("PFM.Website");

    public static Activity? CreateListActivity(string path)
    {
        return Source.CreateActivity($"GET /{path}", ActivityKind.Client);
    }
}