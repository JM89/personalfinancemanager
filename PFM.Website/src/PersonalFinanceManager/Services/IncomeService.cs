using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class IncomeService: IIncomeService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public IncomeService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<bool> CreateIncomes(List<IncomeEditModel> models)
        {
            var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>).ToList();
            return await _httpClientExtended.Post($"/Income/CreateIncomes", dto);
        }

        public async Task<bool> CreateIncome(IncomeEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>(model);
            return await _httpClientExtended.Post($"/Income/Create", dto);
        }

        public async Task<IList<IncomeListModel>> GetIncomes(int accountId)
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Income.IncomeList>($"/Income/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<IncomeListModel>).ToList();
        }

        public async Task<IncomeEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Income.IncomeDetails>($"/Income/Get/{id}");
            return AutoMapper.Mapper.Map<IncomeEditModel>(response);
        }

        public async Task<bool> DeleteIncome(int id)
        {
            return await _httpClientExtended.Delete($"/Income/Delete/{id}");
        }
    }
}