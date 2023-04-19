using PersonalFinanceManager.Models.TaxType;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class TaxTypeService : ITaxTypeService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public TaxTypeService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public IList<TaxTypeListModel> GetTaxTypes()
        {
            IList<TaxTypeListModel> result = null;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.TaxType.TaxTypeList>($"/TaxType/GetList");
            result = response.Select(AutoMapper.Mapper.Map<TaxTypeListModel>).ToList();
            return result;
        }
    }
}