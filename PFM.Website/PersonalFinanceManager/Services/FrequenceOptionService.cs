using PersonalFinanceManager.Models.FrequenceOption;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class FrequenceOptionService : IFrequenceOptionService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public FrequenceOptionService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<IList<FrequenceOptionListModel>> GetFrequencyOptions()
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.FrequenceOption.FrequenceOptionList>($"/FrequenceOption/GetList");
            return response.Select(AutoMapper.Mapper.Map<FrequenceOptionListModel>).ToList();
        }
    }
}