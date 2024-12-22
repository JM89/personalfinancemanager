using System.Diagnostics;
using PFM.Api.Configurations.Monitoring.Metrics;
using Serilog.Context;
using SerilogTimings;

namespace PFM.Api.MiddleWares;

/// <summary>
/// Middleware to track request duration.
/// </summary>
/// <param name="next">Request Delegate</param>
/// <param name="metrics">System Diagnostics Metrics Wrapper.</param>
public class TimedOperationMiddleware(RequestDelegate next, IRequestMetrics metrics)
{
    private const string StatusCodeLogProperty = "StatusCode";

    private readonly List<string> _ignoreRules =
    [
        "/metrics",
        "/metrics-text",
        "/swagger/v1/swagger.json",
        "/swagger/index.html",
        "/swagger",
        "/favicon.ico"
    ];
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Path.HasValue && _ignoreRules.Any(x => httpContext.Request.Path.Value.StartsWith(x)))
        {
            await next(httpContext);
            return;
        }
        
        using var op = Operation.Begin($"{httpContext.Request.Method} {httpContext.Request.Path}");
        
        var stopwatch = Stopwatch.StartNew();
        await next(httpContext);
        
        var tags = BuildMetricsTags(httpContext);
        metrics.IncrementRequestCounter(tags);
        metrics.RecordRequestDuration(stopwatch.ElapsedMilliseconds, tags);
        
        LogContext.PushProperty(StatusCodeLogProperty, httpContext.Response.StatusCode);
        op.Complete();
    }

    private const string ControllerTag = "controller";
    private const string MethodTag = "method";
    private const string StatusCodeTag = "status_code";

    private KeyValuePair<string,object>[] BuildMetricsTags(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value == "" ? "empty_path" : httpContext.Request.Path.Value;
        var method = httpContext.Request.Method == "" ? "empty_method" : httpContext.Request.Method;
        var statusCode = httpContext.Response.StatusCode.ToString() == "" ? "empty_status_code" : httpContext.Response.StatusCode.ToString();
        return
        [
            new KeyValuePair<string, object>(ControllerTag, path),
            new KeyValuePair<string, object>(MethodTag, method),
            new KeyValuePair<string, object>(StatusCodeTag, statusCode)
        ];
    }
}