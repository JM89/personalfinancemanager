using Api.Contracts.Shared;
using AutoMapper;
using Newtonsoft.Json;

namespace PFM.Website.Services
{
	public class CoreService
	{
        protected readonly Serilog.ILogger _logger;
        protected IMapper _mapper;

        public CoreService(Serilog.ILogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        protected TResult? ReadApiResponse<TResult>(ApiResponse apiResponse)
        {
            if (apiResponse.Data == null)
            {
                _logger.Error("No data returned");
                return default(TResult);
            }

            _logger.Information("Read API Response of type {Type}", apiResponse.Data.GetType());

            if (typeof(TResult) == typeof(bool) && bool.TryParse(apiResponse.Data.ToString(), out bool bResult))
            {
                return (TResult)Convert.ChangeType(bResult, typeof(TResult));
            }

            return JsonConvert.DeserializeObject<TResult>(apiResponse.Data.ToString() ?? "");
        }
    }
}

