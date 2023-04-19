using PersonalFinanceManager.Models.Salary;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly Serilog.ILogger _logger;

        public SalaryService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<SalaryListModel> GetSalaries(string userId)
        {
            IList<SalaryListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.Salary.SalaryList>($"/Salary/GetList/{userId}");
                result = response.Select(AutoMapper.Mapper.Map<SalaryListModel>).ToList();
            }
            return result;
        }

        public void CreateSalary(SalaryEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Salary.SalaryDetails>(model);
                httpClient.Post($"/Salary/Create", dto);
            }
        }

        public void CopySalary(int sourceId)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Post($"/Salary/CopySalary/{sourceId}");
            }
        }

        public SalaryEditModel GetById(int id)
        {
            SalaryEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.Salary.SalaryDetails>($"/Salary/Get/{id}");
                result = AutoMapper.Mapper.Map<SalaryEditModel>(response);
            }
            return result;
        }

        public void EditSalary(SalaryEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Salary.SalaryDetails>(model);
                httpClient.Put($"/Salary/Edit/{model.Id}", dto);
            }
        }

        public void DeleteSalary(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/Salary/Delete/{id}");
            }
        }
    }
}