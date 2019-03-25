using System.Collections.Generic;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Interfaces;
using System;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Models.BudgetPlan;
using System.Linq;
using PersonalFinanceManager.Services.HttpClientWrapper;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureService : IExpenditureService
    {
        public void CreateExpenditures(List<ExpenditureEditModel> models)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = models.Select(AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.Expense.ExpenseDetails>).ToList();
                httpClient.Post($"/Expense/CreateExpenses", dto);
            }
        }

        public void CreateExpenditure(ExpenditureEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.Expense.ExpenseDetails>(model);
                httpClient.Post($"/Expense/Create", dto);
            }
        }
        
        public void EditExpenditure(ExpenditureEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.Expense.ExpenseDetails>(model);
                httpClient.Put($"/Expense/Edit/{model.Id}", dto);
            }
        }

        public void DeleteExpenditure(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Expense/Delete/{id}");
            }
        }

        public ExpenditureEditModel GetById(int id)
        {
            ExpenditureEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PersonalFinanceManager.DTOs.Expense.ExpenseDetails>($"/Expense/Get/{id}");
                result = AutoMapper.Mapper.Map<ExpenditureEditModel>(response);
            }
            return result;
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Post($"/Expense/ChangeDebitStatus/{id}/{debitStatus}");
            }
        }

        public IList<ExpenditureListModel> GetExpenditures(Models.SearchParameters.ExpenditureGetListSearchParameters search)
        {
            IList<ExpenditureListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var searchParameters = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.SearchParameters.ExpenseGetListSearchParameters>(search);
                var response = httpClient.GetListBySearchParameters<PersonalFinanceManager.DTOs.Expense.ExpenseList, PersonalFinanceManager.DTOs.SearchParameters.ExpenseGetListSearchParameters>("/Expense/GetExpenses", searchParameters);
                result = response.Select(AutoMapper.Mapper.Map<ExpenditureListModel>).ToList();
            }
            return result;
        }

        public ExpenseSummaryModel GetExpenseSummary(int accountId, BudgetPlanEditModel model)
        {
            ExpenseSummaryModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.BudgetPlan.BudgetPlanDetails>(model);
                var response = httpClient.Post<PersonalFinanceManager.DTOs.BudgetPlan.BudgetPlanDetails, PersonalFinanceManager.DTOs.Dashboard.ExpenseSummary>($"/Expense/GetExpenseSummary/{accountId}", dto);
                result = AutoMapper.Mapper.Map<ExpenseSummaryModel>(response);
            }
            return result;
        }
    }
}