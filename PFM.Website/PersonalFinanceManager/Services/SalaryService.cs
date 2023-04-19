﻿using PersonalFinanceManager.Models.Salary;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public SalaryService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public IList<SalaryListModel> GetSalaries(string userId)
        {
            IList<SalaryListModel> result = null;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Salary.SalaryList>($"/Salary/GetList/{userId}");
            result = response.Select(AutoMapper.Mapper.Map<SalaryListModel>).ToList();
            return result;
        }

        public void CreateSalary(SalaryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Salary.SalaryDetails>(model);
            _httpClientExtended.Post($"/Salary/Create", dto);
        }

        public void CopySalary(int sourceId)
        {
            _httpClientExtended.Post($"/Salary/CopySalary/{sourceId}");
        }

        public SalaryEditModel GetById(int id)
        {
            SalaryEditModel result = null;
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Salary.SalaryDetails>($"/Salary/Get/{id}");
            result = AutoMapper.Mapper.Map<SalaryEditModel>(response);
            return result;
        }

        public void EditSalary(SalaryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Salary.SalaryDetails>(model);
            _httpClientExtended.Put($"/Salary/Edit/{model.Id}", dto);
        }

        public void DeleteSalary(int id)
        {
            _httpClientExtended.Delete($"/Salary/Delete/{id}");
        }
    }
}