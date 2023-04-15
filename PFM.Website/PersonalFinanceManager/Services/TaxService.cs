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
        public void CreateTax(TaxEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Tax.TaxDetails>(model);
                httpClient.Post($"/Tax/Create", dto);
            }
        }

        public void DeleteTax(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Tax/Delete/{id}");
            }
        }

        public void EditTax(TaxEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Tax.TaxDetails>(model);
                httpClient.Put($"/Tax/Edit/{model.Id}", dto);
            }
        }

        public TaxEditModel GetById(int id)
        {
            TaxEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.Tax.TaxDetails>($"/Tax/Get/{id}");
                result = AutoMapper.Mapper.Map<TaxEditModel>(response);
            }
            return result;
        }

        public IList<TaxListModel> GetTaxes(string userId)
        {
            IList<TaxListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetList/{userId}");
                result = response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
            }
            return result;
        }

        public IList<TaxListModel> GetTaxesByType(string currentUser, TaxType incomeTax)
        {
            IList<TaxListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var taxTypeId = (int)TaxType.IncomeTax;
                var response = httpClient.GetList<PFM.Api.Contracts.Tax.TaxList>($"/Tax/GetTaxesByType/{currentUser}/{taxTypeId}");
                result = response.Select(AutoMapper.Mapper.Map<TaxListModel>).ToList();
            }
            return result;
        }
    }
}