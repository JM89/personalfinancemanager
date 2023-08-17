using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PFM.CommonLibraries.Api.Contracts;
using PFM.CommonLibraries.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PFM.CommonLibraries.Api.MiddleWares
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly List<string> ignoreRules = new List<string> {
            "/swagger/v1/swagger.json",
            "/swagger/index.html",
            "/swagger"
        };

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

            try
            {
                if (context.Request.Path.HasValue && ignoreRules.Any(x => context.Request.Path.Value.StartsWith(x)))
                {
                    await _next(context);
                    return;
                }
            }
            catch (Exception ex)
            {
                await HandleExceptions(context, ex);
            }

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                try
                {
                    await _next(context);

                    context.Response.Body = currentBody;
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var readToEnd = new StreamReader(memoryStream).ReadToEnd();

                    // When calls are forwarded to another API and API response is returned.
                    var apiResponse = new ApiResponse();
                    try
                    {
                        apiResponse = JsonConvert.DeserializeObject<ApiResponse>(readToEnd);
                    }
                    catch (Exception) { }

                    if (apiResponse?.Data == null && apiResponse?.Errors == null)
                    {
                        var objResult = JsonConvert.DeserializeObject(readToEnd);
                        apiResponse = new ApiResponse(objResult);
                    }

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse, _serializeOptions));
                }
                catch (BusinessException ex)
                {
                    context.Response.Body = currentBody;
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(ex.ErrorMessages), _serializeOptions));
                }
                catch (Exception ex)
                {
                    context.Response.Body = currentBody;
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    await HandleExceptions(context, ex);
                }
            }
        }

        private async Task HandleExceptions(HttpContext context, Exception ex)
        {
            _logger.Error(ex, "Unhandled exception occurred");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse("Unhandled exception occurred"), _serializeOptions));
        }
    }
}
