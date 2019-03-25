using System;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureTypeService : IExpenditureTypeService
    {
        public IList<ExpenditureTypeListModel> GetExpenditureTypes()
        {
            IList<ExpenditureTypeListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeList>($"/ExpenseType/GetList");
                result = response.Select(AutoMapper.Mapper.Map<ExpenditureTypeListModel>).ToList();
            }
            return result;
        }

        public ExpenditureTypeEditModel GetById(int id)
        {
            ExpenditureTypeEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeDetails>($"/ExpenseType/Get/{id}");
                result = AutoMapper.Mapper.Map<ExpenditureTypeEditModel>(response);
            }
            return result;
        }

        public void CreateExpenditureType(ExpenditureTypeEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeDetails>(model);
                httpClient.Post($"/ExpenseType/Create", dto);
            }
        }

        public void EditExpenditureType(ExpenditureTypeEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeDetails>(model);
                httpClient.Put($"/ExpenseType/Edit/{model.Id}", dto);
            }
        }

        public void DeleteExpenditureType(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/ExpenseType/Delete/{id}");
            }
        }
    }
}