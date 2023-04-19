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
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Tax.TaxDetails>($"/Tax/Get/{id}");
            return AutoMapper.Mapper.Map<TaxEditModel>(response);
        }

        public IList<TaxListModel> GetTaxes(string userId)
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetList/{userId}");
            return response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
        }

        public IList<TaxListModel> GetTaxesByType(string currentUser, TaxType incomeTax)
        {
            var taxTypeId = (int)TaxType.IncomeTax;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetTaxesByType/{currentUser}/{taxTypeId}");
            return response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
        }
    }
}