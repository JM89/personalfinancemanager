using System.Collections.Generic;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Interfaces;
using System;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Models.BudgetPlan;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureService : IExpenditureService
    {
        public void CreateExpenditures(List<ExpenditureEditModel> expenditureEditModel)
        {
            throw new NotImplementedException();
        }

        public void CreateExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            throw new NotImplementedException();
        }
        
        public void EditExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteExpenditure(int id)
        {
            throw new NotImplementedException();
        }

        public ExpenditureEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            throw new NotImplementedException();
        }

        public IList<ExpenditureListModel> GetExpenditures(Models.SearchParameters.ExpenditureGetListSearchParameters search)
        {
            throw new NotImplementedException();
        }

        public ExpenseSummaryModel GetExpenseSummary(int accountId, BudgetPlanEditModel budgetPlan)
        {
            throw new NotImplementedException();
        }
    }
}