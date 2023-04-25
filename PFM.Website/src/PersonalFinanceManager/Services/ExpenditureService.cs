using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public ExpenditureService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<bool> CreateExpenditures(List<ExpenditureEditModel> models)
        {
            var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.Expense.ExpenseDetails>).ToList();
            return await _httpClientExtended.Post($"/Expense/CreateExpenses", dto);
        }

        public async Task<bool> CreateExpenditure(ExpenditureEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Expense.ExpenseDetails>(model);
            return await _httpClientExtended.Post($"/Expense/Create", dto);
        }
        
        public async Task<bool> EditExpenditure(ExpenditureEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Expense.ExpenseDetails>(model);
            return await _httpClientExtended.Put($"/Expense/Edit/{model.Id}", dto);
        }

        public async Task<bool> DeleteExpenditure(int id)
        {
            return await _httpClientExtended.Delete($"/Expense/Delete/{id}");
        }

        public async Task<ExpenditureEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Expense.ExpenseDetails>($"/Expense/Get/{id}");
            return AutoMapper.Mapper.Map<ExpenditureEditModel>(response);
        }

        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await _httpClientExtended.Post($"/Expense/ChangeDebitStatus/{id}/{debitStatus}");
        }

        public async Task<IList<ExpenditureListModel>> GetExpenditures(Models.SearchParameters.ExpenditureGetListSearchParameters search)
        {
            var searchParameters = AutoMapper.Mapper.Map<PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters>(search);
            var response = await _httpClientExtended.GetListBySearchParameters<PFM.Api.Contracts.Expense.ExpenseList, PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters>("/Expense/GetExpenses", searchParameters);
            return response.Select(AutoMapper.Mapper.Map<ExpenditureListModel>).ToList();
        }

        public async Task<ExpenseSummaryModel> GetExpenseSummary(int accountId, BudgetPlanEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
            var response = await _httpClientExtended.Post<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails, PFM.Api.Contracts.Dashboard.ExpenseSummary>($"/Expense/GetExpenseSummary/{accountId}", dto);
            return AutoMapper.Mapper.Map<ExpenseSummaryModel>(response);
        }
    }
}