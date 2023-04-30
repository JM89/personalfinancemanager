using Microsoft.AspNetCore.Http;
using PFM.Api.Contracts.Shared;
using PFM.Services.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace PFM.Api.Middlewares
{
    public class UnhandledExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        private readonly JsonSerializerOptions _serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public UnhandledExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BusinessException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse(ex.ErrorMessages), _serializeOptions));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred");
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse("Unhandled exception occurred"), _serializeOptions));
            }
        }
    }
}
