using System;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Models.TaxType;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class TaxTypeService : ITaxTypeService
    {
        public IList<TaxTypeListModel> GetTaxTypes()
        {
            IList<TaxTypeListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.Api.Contracts.TaxType.TaxTypeList>($"/TaxType/GetList");
                result = response.Select(AutoMapper.Mapper.Map<TaxTypeListModel>).ToList();
            }
            return result;
        }
    }
}