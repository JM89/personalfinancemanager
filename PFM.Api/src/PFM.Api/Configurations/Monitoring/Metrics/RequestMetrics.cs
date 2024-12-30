using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PFM.UnitTests")]

namespace PFM.Api.Configurations.Monitoring.Metrics;

public interface IRequestMetrics
{
    void IncrementRequestCounter(KeyValuePair<string, object>[] tags);
    void RecordRequestDuration(long stopwatchElapsedMilliseconds, KeyValuePair<string, object>[] tags);
}

internal class RequestMetrics : IRequestMetrics
{
    internal static readonly string MeterName = "pfm.requests";
    private readonly Histogram<double> _histogram;
    private readonly Counter<int> _counter;

    public RequestMetrics()
    {
        var meter = new Meter(MeterName);
        _histogram = meter.CreateHistogram<double>("http_request_timer", unit: "ms");
        _counter = meter.CreateCounter<int>("http_request_counter", unit: "items");
    }

    public void IncrementRequestCounter(KeyValuePair<string, object>[] tags)
    {
        _counter.Add(1, tags!);
    }

    public void RecordRequestDuration(long stopwatchElapsedMilliseconds, KeyValuePair<string, object>[] tags)
    {
        _histogram.Record(stopwatchElapsedMilliseconds, tags!);
    }
}