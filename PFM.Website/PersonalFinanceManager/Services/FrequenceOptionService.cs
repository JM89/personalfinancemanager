using System;
using System.Collections.Generic;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.FrequenceOption;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class FrequenceOptionService : IFrequenceOptionService
    {
        public IList<FrequenceOptionListModel> GetFrequencyOptions()
        {
            IList<FrequenceOptionListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.Api.Contracts.FrequenceOption.FrequenceOptionList>($"/FrequenceOption/GetList");
                result = response.Select(AutoMapper.Mapper.Map<FrequenceOptionListModel>).ToList();
            }
            return result;
        }
    }
}