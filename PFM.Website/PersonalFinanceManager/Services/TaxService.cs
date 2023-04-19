using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using PersonalFinanceManager.Models.Tax;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;
using PersonalFinanceManager.Models.TaxType;

namespace PersonalFinanceManager.Services
{
    public class TaxService : ITaxService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public TaxService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public void CreateTax(TaxEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Tax.TaxDetails>(model);
            _httpClientExtended.Post($"/Tax/Create", dto);
        }

        public void DeleteTax(int id)
        {
            _httpClientExtended.Delete($"/Tax/Delete/{id}");
        }

        public void EditTax(TaxEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Tax.TaxDetails>(model);
            _httpClientExtended.Put($"/Tax/Edit/{model.Id}", dto);
        }

        public TaxEditModel GetById(int id)
        {
            TaxEditModel result = null;
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Tax.TaxDetails>($"/Tax/Get/{id}");
            result = AutoMapper.Mapper.Map<TaxEditModel>(response);
            return result;
        }

        public IList<TaxListModel> GetTaxes(string userId)
        {
            IList<TaxListModel> result = null;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetList/{userId}");
            result = response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
            return result;
        }

        public IList<TaxListModel> GetTaxesByType(string currentUser, TaxType incomeTax)
        {
            IList<TaxListModel> result = null;
            var taxTypeId = (int)TaxType.IncomeTax;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetTaxesByType/{currentUser}/{taxTypeId}");
            result = response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
            return result;
        }
    }
}