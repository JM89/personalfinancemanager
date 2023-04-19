using PersonalFinanceManager.Models.FrequenceOption;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class FrequenceOptionService : IFrequenceOptionService
    {
        private readonly Serilog.ILogger _logger;

        public FrequenceOptionService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<FrequenceOptionListModel> GetFrequencyOptions()
        {
            IList<FrequenceOptionListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.FrequenceOption.FrequenceOptionList>($"/FrequenceOption/GetList");
                result = response.Select(AutoMapper.Mapper.Map<FrequenceOptionListModel>).ToList();
            }
            return result;
        }
    }
}