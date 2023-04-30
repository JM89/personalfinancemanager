using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IExpenditureService : IBaseService
    {
        Task<bool> CreateExpenditures(List<ExpenditureEditModel> expenditureEditModel);

        Task<bool> CreateExpenditure(ExpenditureEditModel expenditureEditModel);

        Task<bool> DeleteExpenditure(int id);

        Task<ExpenditureEditModel> GetById(int id);

        Task<bool> ChangeDebitStatus(int id, bool debitStatus);

        Task<ExpenseSummaryModel> GetExpenseSummary(int accountId, BudgetPlanEditModel budgetPlan);

        Task<IList<ExpenditureListModel>> GetExpenditures(Models.SearchParameters.ExpenditureGetListSearchParameters search);
    }
}