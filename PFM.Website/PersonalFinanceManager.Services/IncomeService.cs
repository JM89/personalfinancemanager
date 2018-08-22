using System.Collections.Generic;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Interfaces;
using System;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class IncomeService: IIncomeService
    {
        public void CreateIncomes(List<IncomeEditModel> models)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = models.Select(AutoMapper.Mapper.Map<PFM.DTOs.Income.IncomeDetails>).ToList();
                httpClient.Post($"/Income/CreateIncomes", dto);
            }
        }

        public void CreateIncome(IncomeEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Income.IncomeDetails>(model);
                httpClient.Post($"/Income/Create", dto);
            }
        }

        public IList<IncomeListModel> GetIncomes(int accountId)
        {
            IList<IncomeListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.DTOs.Income.IncomeList>($"/Income/GetList");
                result = response.Select(AutoMapper.Mapper.Map<IncomeListModel>).ToList();
            }
            return result;
        }

        public IncomeEditModel GetById(int id)
        {
            IncomeEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.DTOs.Income.IncomeDetails>($"/Income/Get/{id}");
                result = AutoMapper.Mapper.Map<IncomeEditModel>(response);
            }
            return result;
        }

        public void EditIncome(IncomeEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Income.IncomeDetails>(model);
                httpClient.Put($"/Income/Edit/{model.Id}", dto);
            }
        }

        public void DeleteIncome(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Income/Delete/{id}");
            }
        }
    }
}