using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public IList<BudgetPlanListModel> GetBudgetPlans(int accountId)
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.BudgetPlan.BudgetPlanList>($"/BudgetPlan/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<BudgetPlanListModel>).ToList();
        }
        
        public BudgetPlanEditModel GetCurrent(int accountId)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>($"/BudgetPlan/GetCurrent/{accountId}");
            return AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
        }

        public BudgetPlanEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>($"/BudgetPlan/Get/{id}");
            return AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
        }

        public void CreateBudgetPlan(BudgetPlanEditModel model, int accountId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
            _httpClientExtended.Post($"/BudgetPlan/Create/{accountId}", dto);
        }

        public void EditBudgetPlan(BudgetPlanEditModel model, int accountId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
            _httpClientExtended.Put($"/BudgetPlan/Edit/{accountId}", dto);
        }

        public void StartBudgetPlan(int value, int accountId)
        {
            _httpClientExtended.Post($"/BudgetPlan/Start/{value}/{accountId}");
        }

        public void StopBudgetPlan(int value)
        {
            _httpClientExtended.Post($"/BudgetPlan/Stop/{value}");
        }

        public BudgetPlanEditModel BuildBudgetPlan(int accountId, int? budgetPlanId = null)
        {
            var url = budgetPlanId.HasValue ? $"/BudgetPlan/BuildEmpty/{accountId}/{budgetPlanId}" : $"/BudgetPlan/BuildEmpty/{accountId}";
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(url);
            return AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
        }
    }
}