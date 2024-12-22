using System.Diagnostics;

namespace PFM.MovementAggregator.Monitoring.Tracing;

public static class CustomActivitySource
{
    internal static readonly ActivitySource Source = new ("PFM.MovementAggregator");

    public static Activity? StartDispatcherActivity(string eventType, string? traceId)
    {
        return Source.StartActivity($"Dispatcher-{eventType}", ActivityKind.Internal, traceId);
    }
}