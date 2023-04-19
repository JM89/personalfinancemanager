using PersonalFinanceManager.Models.Salary;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IList<SalaryListModel>> GetSalaries(string userId)
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Salary.SalaryList>($"/Salary/GetList/{userId}");
            return response.Select(AutoMapper.Mapper.Map<SalaryListModel>).ToList();
        }

        public async Task<bool> CreateSalary(SalaryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Salary.SalaryDetails>(model);
            return await _httpClientExtended.Post($"/Salary/Create", dto);
        }

        public async Task<bool> CopySalary(int sourceId)
        {
            return await _httpClientExtended.Post($"/Salary/CopySalary/{sourceId}");
        }

        public async Task<SalaryEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Salary.SalaryDetails>($"/Salary/Get/{id}");
            return AutoMapper.Mapper.Map<SalaryEditModel>(response);
        }

        public async Task<bool> EditSalary(SalaryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Salary.SalaryDetails>(model);
            return await _httpClientExtended.Put($"/Salary/Edit/{model.Id}", dto);
        }

        public async Task<bool> DeleteSalary(int id)
        {
            return await _httpClientExtended.Delete($"/Salary/Delete/{id}");
        }
    }
}