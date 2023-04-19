using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public void CreateExpenditures(List<ExpenditureEditModel> models)
        {
            var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.Expense.ExpenseDetails>).ToList();
            _httpClientExtended.Post($"/Expense/CreateExpenses", dto);
        }

        public void CreateExpenditure(ExpenditureEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Expense.ExpenseDetails>(model);
            _httpClientExtended.Post($"/Expense/Create", dto);
        }
        
        public void EditExpenditure(ExpenditureEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Expense.ExpenseDetails>(model);
            _httpClientExtended.Put($"/Expense/Edit/{model.Id}", dto);
        }

        public void DeleteExpenditure(int id)
        {
            _httpClientExtended.Delete($"/Expense/Delete/{id}");
        }

        public ExpenditureEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Expense.ExpenseDetails>($"/Expense/Get/{id}");
            return AutoMapper.Mapper.Map<ExpenditureEditModel>(response);
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            _httpClientExtended.Post($"/Expense/ChangeDebitStatus/{id}/{debitStatus}");
        }

        public IList<ExpenditureListModel> GetExpenditures(Models.SearchParameters.ExpenditureGetListSearchParameters search)
        {
            var searchParameters = AutoMapper.Mapper.Map<PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters>(search);
            var response = _httpClientExtended.GetListBySearchParameters<PFM.Api.Contracts.Expense.ExpenseList, PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters>("/Expense/GetExpenses", searchParameters);
            return response.Select(AutoMapper.Mapper.Map<ExpenditureListModel>).ToList();
        }

        public ExpenseSummaryModel GetExpenseSummary(int accountId, BudgetPlanEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>(model);
            var response = _httpClientExtended.Post<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails, PFM.Api.Contracts.Dashboard.ExpenseSummary>($"/Expense/GetExpenseSummary/{accountId}", dto);
            return AutoMapper.Mapper.Map<ExpenseSummaryModel>(response);
        }
    }
}