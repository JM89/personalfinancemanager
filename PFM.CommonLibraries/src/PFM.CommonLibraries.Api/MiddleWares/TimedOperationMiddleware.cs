using App.Metrics;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Timer;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using SerilogTimings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.CommonLibraries.Api.MiddleWares
{
    public class TimedOperationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMetrics _metrics;

        private readonly string StatusCodeLogProperty = "StatusCode";
        private readonly List<string> ignoreRules = new List<string> {
            "/metrics",
            "/metrics-text",
            "/swagger/v1/swagger.json",
            "/swagger/index.html",
            "/swagger",
            "/favicon.ico"
        };

        private TimerOptions _timerOptions;
        private string[] _metricsKeys = new string[] { "controller", "method", "status_code" };

        public TimedOperationMiddleware(RequestDelegate next, IMetrics metrics)
        {
            _next = next;
            _metrics = metrics;
            _timerOptions = new TimerOptions
            {
                Name = "HTTP Request Timer",
                MeasurementUnit = Unit.Items,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds,
                Reservoir = () => new DefaultForwardDecayingReservoir(sampleSize: 1028, alpha: 0.015)
            };
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.HasValue && ignoreRules.Any(x => httpContext.Request.Path.Value.StartsWith(x)))
            {
                await _next(httpContext);
                return;
            }

            using (_metrics.Measure.Timer.Time(_timerOptions))
            using (var op = Operation.Begin($"{httpContext.Request.Method} {httpContext.Request.Path}"))
            {
                await _next(httpContext);

                _timerOptions.Tags = new MetricTags(_metricsKeys, new string[] { httpContext.Request.Path, httpContext.Request.Method, httpContext.Response.StatusCode.ToString() });

                LogContext.PushProperty(StatusCodeLogProperty, httpContext.Response.StatusCode);
                op.Complete();
            }
        }
    }
}
