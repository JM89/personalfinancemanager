using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using PersonalFinanceManager.Models.Tax;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;
using PersonalFinanceManager.Models.TaxType;
using System.Threading.Tasks;

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

        public async Task<bool> CreateTax(TaxEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Tax.TaxDetails>(model);
            return await _httpClientExtended.Post($"/Tax/Create", dto);
        }

        public async Task<bool> DeleteTax(int id)
        {
            return await _httpClientExtended.Delete($"/Tax/Delete/{id}");
        }

        public async Task<bool> EditTax(TaxEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Tax.TaxDetails>(model);
            return await _httpClientExtended.Put($"/Tax/Edit/{model.Id}", dto);
        }

        public async Task<TaxEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Tax.TaxDetails>($"/Tax/Get/{id}");
            return AutoMapper.Mapper.Map<TaxEditModel>(response);
        }

        public async Task<IList<TaxListModel>> GetTaxes(string userId)
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetList/{userId}");
            return response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
        }

        public async Task<IList<TaxListModel>> GetTaxesByType(string currentUser, TaxType incomeTax)
        {
            var taxTypeId = (int)TaxType.IncomeTax;
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetTaxesByType/{currentUser}/{taxTypeId}");
            return response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
        }
    }
}