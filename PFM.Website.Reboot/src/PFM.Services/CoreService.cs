using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PFM.Api.Contracts.Shared;

namespace PFM.Services
{
    /// <summary>
    /// Base class that contains shared behaviors for all services.
    /// </summary>
	public abstract class CoreService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        protected readonly Serilog.ILogger Logger = logger;
        protected readonly IMapper Mapper = mapper;

        protected TResult? ReadApiResponse<TResult>(ApiResponse apiResponse)
        {
            if (apiResponse.Data == null)
            {
                var flattenErrors = string.Join('-', apiResponse.Errors?.Select(x => $"{x.Key}-{string.Join(';', x.Value)}") ?? new List<string>() { "No errors" });

                Logger.Error(flattenErrors);
                return default(TResult);
            }

            if (typeof(TResult) == typeof(bool) && bool.TryParse(apiResponse.Data.ToString(), out bool bResult))
            {
                return (TResult)Convert.ChangeType(bResult, typeof(TResult));
            }

            return JsonConvert.DeserializeObject<TResult>(apiResponse.Data.ToString() ?? "");
        }

        protected string GetCurrentUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var userId = user?.Claims.SingleOrDefault(x => x.Type == "preferred_username")
                    ?.Value ?? user?.Identity?.Name ?? "Unknown";
            return userId;
        }
    }
}

