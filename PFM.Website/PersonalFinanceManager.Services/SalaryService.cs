﻿using System.Collections.Generic;
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
                var response = httpClient.GetList<PFM.DTOs.Salary.SalaryList>($"/Salary/GetList");
                result = response.Select(AutoMapper.Mapper.Map<SalaryListModel>).ToList();
            }
            return result;
        }

        public void CreateSalary(SalaryEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Salary.SalaryDetails>(model);
                httpClient.Post($"/Salary/Create", dto);
            }
        }

        public void CopySalary(int sourceId)
        {
            // API NOT IMPLEMENTED
            throw new NotImplementedException();
        }

        public SalaryEditModel GetById(int id)
        {
            SalaryEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.DTOs.Salary.SalaryDetails>($"/Salary/Get/{id}");
                result = AutoMapper.Mapper.Map<SalaryEditModel>(response);
            }
            return result;
        }

        public void EditSalary(SalaryEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Salary.SalaryDetails>(model);
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