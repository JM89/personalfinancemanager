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

        public TaxTypeService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<TaxTypeListModel> GetTaxTypes()
        {
            IList<TaxTypeListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.TaxType.TaxTypeList>($"/TaxType/GetList");
                result = response.Select(AutoMapper.Mapper.Map<TaxTypeListModel>).ToList();
            }
            return result;
        }
    }
}