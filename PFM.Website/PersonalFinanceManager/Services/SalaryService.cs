using System.Collections.Generic;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Salary;
using System;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class SalaryService : ISalaryService
    {
        public IList<SalaryListModel> GetSalaries(string userId)
        {
            IList<SalaryListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PersonalFinanceManager.Api.Contracts.Salary.SalaryList>($"/Salary/GetList/{userId}");
                result = response.Select(AutoMapper.Mapper.Map<SalaryListModel>).ToList();
            }
            return result;
        }

        public void CreateSalary(SalaryEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.Api.Contracts.Salary.SalaryDetails>(model);
                httpClient.Post($"/Salary/Create", dto);
            }
        }

        public void CopySalary(int sourceId)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Post($"/Salary/CopySalary/{sourceId}");
            }
        }

        public SalaryEditModel GetById(int id)
        {
            SalaryEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PersonalFinanceManager.Api.Contracts.Salary.SalaryDetails>($"/Salary/Get/{id}");
                result = AutoMapper.Mapper.Map<SalaryEditModel>(response);
            }
            return result;
        }

        public void EditSalary(SalaryEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.Api.Contracts.Salary.SalaryDetails>(model);
                httpClient.Put($"/Salary/Edit/{model.Id}", dto);
            }
        }

        public void DeleteSalary(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Salary/Delete/{id}");
            }
        }
    }
}