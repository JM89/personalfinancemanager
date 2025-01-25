using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using PFM.Bank.Api.Contracts.Shared;
using PFM.Services.Core.Exceptions;

namespace Api.Middlewares;

public class ResponseWrapperMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;
    private readonly List<string> _ignoreRules =
    [
        "/swagger/v1/swagger.json",
        "/swagger/index.html",
        "/swagger"
    ];

    private readonly JsonSerializerSettings _serializeOptions = new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public ResponseWrapperMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var currentBody = context.Response.Body;
        var defaultStatusCode = HttpStatusCode.InternalServerError;

        try
        {
            if (context.Request.Path.HasValue && _ignoreRules.Any(x => context.Request.Path.Value.StartsWith(x)))
            {
                await _next(context);
                return;
            }
        }
        catch (Exception ex)
        {
            await HandleExceptions(context, ex, defaultStatusCode);
        }

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                if (context.Response.StatusCode == 401)
                {
                    defaultStatusCode = HttpStatusCode.Unauthorized;
                    throw new AuthenticationFailureException("Check that the token is still valid.");
                }
                
                context.Response.Body = currentBody;
                memoryStream.Seek(0, SeekOrigin.Begin);

                var readToEnd = new StreamReader(memoryStream).ReadToEnd();

                var objResult = JsonConvert.DeserializeObject(readToEnd);
                var apiResponse = new ApiResponse(objResult);

                await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse, _serializeOptions));
            }
            catch (BusinessException ex)
            {
                context.Response.Body = currentBody;
                memoryStream.Seek(0, SeekOrigin.Begin);
                
                await HandleExceptions(context, ex, HttpStatusCode.BadRequest, ex.ErrorMessages);
            }
            catch (Exception ex)
            {
                context.Response.Body = currentBody;
                memoryStream.Seek(0, SeekOrigin.Begin);

                await HandleExceptions(context, ex, defaultStatusCode);
            }
        }
    }
    
    private async Task HandleExceptions(HttpContext context, Exception ex, HttpStatusCode statusCode, IDictionary<string, List<string>>? errors = null)
    {
        _logger.Error(ex, "Unhandled exception");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var apiResponse = new ApiResponse(ex.GetType().Name);
        if (errors != null && errors.Any())
        {
            apiResponse = new ApiResponse(errors);
        }
        
        await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse, _serializeOptions));
    }
}