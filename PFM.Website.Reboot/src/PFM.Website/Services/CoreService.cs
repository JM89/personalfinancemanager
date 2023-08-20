using Api.Contracts.Shared;
using AutoMapper;
using Newtonsoft.Json;
using PFM.Website.Configurations;

namespace PFM.Website.Services
{
	public class CoreService
	{
        protected readonly Serilog.ILogger _logger;
        protected IMapper _mapper;
        protected readonly ApplicationSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CoreService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationSettings settings)
        {
            _logger = logger;
            _mapper = mapper;
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
        }

        protected TResult? ReadApiResponse<TResult>(ApiResponse apiResponse)
        {
            if (apiResponse.Data == null)
            {
                var flattenErrors = String.Join('-', apiResponse.Errors?.Select(x => $"{x.Key}-{String.Join(';', x.Value)}") ?? new List<string>() { "No errors" });

                _logger.Error(flattenErrors);
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
            var userName = _httpContextAccessor.HttpContext?.User?.Claims.SingleOrDefault(x => x.Type == "preferred_username")?.Value;

            if (userName != null) return userName;

            userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (userName != null) return userName;

            return "No current user id";
        }
    }
}

