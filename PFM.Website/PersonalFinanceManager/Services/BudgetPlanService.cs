using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class BudgetPlanService : IBudgetPlanService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public BudgetPlanService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<IList<BudgetPlanListModel>> GetBudgetPlans(int accountId)
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.BudgetPlan.BudgetPlanList>($"/BudgetPlan/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<BudgetPlanListModel>).ToList();
        }
        
        public async Task<BudgetPlanEditModel> GetCurrent(int accountId)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>($"/BudgetPlan/GetCurrent/{accountId}");
            return AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
        }

        public async Task<BudgetPlanEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>($"/BudgetPlan/Get/{id}");
            return AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
        }

        public async Task<bool> CreateBudgetPlan(BudgetPlanEditModel model, int accountId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
            return await _httpClientExtended.Post($"/BudgetPlan/Create/{accountId}", dto);
        }

        public async Task<bool> EditBudgetPlan(BudgetPlanEditModel model, int accountId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
            return await _httpClientExtended.Put($"/BudgetPlan/Edit/{accountId}", dto);
        }

        public async Task<bool> StartBudgetPlan(int value, int accountId)
        {
            return await _httpClientExtended.Post($"/BudgetPlan/Start/{value}/{accountId}");
        }

        public async Task<bool> StopBudgetPlan(int value)
        {
            return await _httpClientExtended.Post($"/BudgetPlan/Stop/{value}");
        }

        public async Task<BudgetPlanEditModel> BuildBudgetPlan(int accountId, int? budgetPlanId = null)
        {
            var url = budgetPlanId.HasValue ? $"/BudgetPlan/BuildEmpty/{accountId}/{budgetPlanId}" : $"/BudgetPlan/BuildEmpty/{accountId}";
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(url);
            return AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
        }
    }
}