using Serilog.Context;
using SerilogTimings;

namespace PFM.Api.Middlewares
{
    public class TimedOperationMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly string StatusCodeLogProperty = "StatusCode";

        public TimedOperationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (var op = Operation.Begin($"{httpContext.Request.Method} {httpContext.Request.Path}"))
            {
                await _next(httpContext);

                LogContext.PushProperty(StatusCodeLogProperty, httpContext.Response.StatusCode);
                op.Complete();
            }
        }
    }
}
