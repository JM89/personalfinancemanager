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

        public BudgetPlanService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<BudgetPlanListModel> GetBudgetPlans(int accountId)
        {
            IList<BudgetPlanListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.BudgetPlan.BudgetPlanList>($"/BudgetPlan/GetList/{accountId}");
                result = response.Select(AutoMapper.Mapper.Map<BudgetPlanListModel>).ToList();
            }
            return result;
        }
        
        public BudgetPlanEditModel GetCurrent(int accountId)
        {
            BudgetPlanEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>($"/BudgetPlan/GetCurrent/{accountId}");
                result = AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
            }
            return result;
        }

        public BudgetPlanEditModel GetById(int id)
        {
            BudgetPlanEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>($"/BudgetPlan/Get/{id}");
                result = AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
            }
            return result;
        }

        public void CreateBudgetPlan(BudgetPlanEditModel model, int accountId)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
                httpClient.Post($"/BudgetPlan/Create/{accountId}", dto);
            }
        }

        public void EditBudgetPlan(BudgetPlanEditModel model, int accountId)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
                httpClient.Put($"/BudgetPlan/Edit/{accountId}", dto);
            }
        }

        public void StartBudgetPlan(int value, int accountId)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Post($"/BudgetPlan/Start/{value}/{accountId}");
            }
        }

        public void StopBudgetPlan(int value)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Post($"/BudgetPlan/Stop/{value}");
            }
        }

        public BudgetPlanEditModel BuildBudgetPlan(int accountId, int? budgetPlanId = null)
        {
            BudgetPlanEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var url = budgetPlanId.HasValue ? $"/BudgetPlan/BuildEmpty/{accountId}/{budgetPlanId}" : $"/BudgetPlan/BuildEmpty/{accountId}";
                var response = httpClient.GetSingle<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(url);
                result = AutoMapper.Mapper.Map<BudgetPlanEditModel>(response);
            }
            return result;
        }
    }
}