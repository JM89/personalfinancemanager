using Microsoft.AspNetCore.Http;
using SerilogTimings;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Middlewares
{
    public class TimedOperationMiddleware
    {
        private readonly RequestDelegate _next;

        public TimedOperationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (var op = Operation.Begin($"{httpContext.Request.Method} {httpContext.Request.Path}"))
            {
                await _next(httpContext);
                op.Complete();
            }
        }
    }
}
