﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PFM.Api.Contracts.Shared;
using PFM.Services.Core.Exceptions;
using System.IO;
using System.Net;

namespace PFM.Api.Middlewares
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly List<string> ignoreRules = new List<string> {
            "/swagger/v1/swagger.json"
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

            if (ignoreRules.Contains(context.Request.Path) || currentBody.GetType().Name != "HttpResponseStream")
            {
                await _next(context);
                return;
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
                    var objResult = JsonConvert.DeserializeObject(readToEnd);
                    var result = new ApiResponse(objResult);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result, _serializeOptions));
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

                    _logger.Error(ex, "Unhandled exception occurred");
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse("Unhandled exception occurred"), _serializeOptions));
                }
            }
        }
    }
}