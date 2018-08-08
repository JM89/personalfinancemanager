using System.Collections.Generic;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Core;
using PersonalFinanceManager.Services.RequestObjects;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IExpenditureService : IBaseService
    {
        void CreateExpenditures(List<ExpenditureEditModel> expenditureEditModel);

        void CreateExpenditure(ExpenditureEditModel expenditureEditModel);

        void EditExpenditure(ExpenditureEditModel expenditureEditModel);

        void DeleteExpenditure(int id);

        ExpenditureEditModel GetById(int id);

        void ChangeDebitStatus(int id, bool debitStatus);

        ExpenseSummaryModel GetExpenseSummary(int accountId, BudgetPlanEditModel budgetPlan);

        IList<ExpenditureListModel> GetExpenditures(Models.SearchParameters.ExpenditureGetListSearchParameters search);
    }
}