using AutoMapper;
using Newtonsoft.Json;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Dashboard;
using PFM.Api.Contracts.Expense;
using PFM.Bank.Api.Contracts.Account;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.ExternalServices.BankApi;
using PFM.Services.MovementStrategy;
using PFM.Services.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PFM.Services.Interfaces.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IBankAccountApi _bankAccountApi;
        private readonly ContextMovementStrategy _contextMovementStrategy;

        public ExpenseService(IExpenseRepository ExpenseRepository, IBankAccountApi bankAccountApi, ContextMovementStrategy contextMovementStrategy)
        {
            this._expenseRepository = ExpenseRepository;
            this._bankAccountApi = bankAccountApi;
            this._contextMovementStrategy = contextMovementStrategy;
        }

        public async Task<bool> CreateExpenses(List<ExpenseDetails> ExpenseDetails)
        {
            var resultBatch = true;

            foreach (var expense in ExpenseDetails)
            {
                var result = await CreateExpense(expense);
                if (!result)
                    resultBatch = false;
            }

            return resultBatch;
        }

        public async Task<bool> CreateExpense(ExpenseDetails expenseDetails)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var expense = Mapper.Map<Expense>(expenseDetails);

                var movement = new Movement(expenseDetails);

                var strategy = _contextMovementStrategy.GetMovementStrategy(movement.PaymentMethod);
                var result = await strategy.Debit(movement);

                if (movement.TargetIncomeId.HasValue)
                    expense.GeneratedIncomeId = movement.TargetIncomeId.Value;

                _expenseRepository.Create(expense);

                scope.Complete();

                return result;
            }
        }
        
        public async Task<bool> DeleteExpense(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var expense = _expenseRepository.GetById(id);
                var expenseDetails = Mapper.Map<ExpenseDetails>(expense);

                _expenseRepository.Delete(expense);

                var strategy = _contextMovementStrategy.GetMovementStrategy((PFM.DataAccessLayer.Enumerations.PaymentMethod)expenseDetails.PaymentMethodId);
                var result = await strategy.Credit(new Movement(expenseDetails));

                scope.Complete();

                return result;
            }
        }

        public Task<ExpenseDetails> GetById(int id)
        {
            var expense = _expenseRepository
                            .GetList2(u => u.ExpenseType, u => u.PaymentMethod)
                            .SingleOrDefault(x => x.Id == id);

            if (expense == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<ExpenseDetails>(expense));
        }

        public Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            var Expense = _expenseRepository.GetById(id);
            Expense.HasBeenAlreadyDebited = debitStatus;
            _expenseRepository.Update(Expense);
            return Task.FromResult(true);
        }

        public async Task<IList<ExpenseList>> GetExpenses(PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search)
        {
            var searchParameters = Mapper.Map<PFM.DataAccessLayer.SearchParameters.ExpenseGetListSearchParameters>(search);
            var Expenses = _expenseRepository.GetByParameters(searchParameters).ToList();

            if (!string.IsNullOrEmpty(search.UserId))
            {
                var accountsForUserResponse = await _bankAccountApi.GetList(search.UserId);
                var accountsForUser = JsonConvert.DeserializeObject<List<AccountDetails>>(accountsForUserResponse.Data.ToString());
                var filterAccounts = accountsForUser.Select(x => x.Id);

                Expenses = Expenses.Where(x => filterAccounts.Contains(x.AccountId)).ToList();
            }

            var mappedExpenses = Expenses.Select(Mapper.Map<ExpenseList>);
            return mappedExpenses.ToList();
        }
    }
}