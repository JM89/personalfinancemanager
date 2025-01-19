using AutoMapper;
using Newtonsoft.Json;
using PFM.Api.Contracts.Expense;
using PFM.Bank.Api.Contracts.Account;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.ExternalServices.BankApi;
using PFM.Services.MovementStrategy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PFM.Services.Interfaces.Services
{
    public class ExpenseService(
        IMapper mapper,
        IExpenseRepository expenseRepository,
        IBankAccountApi bankAccountApi,
        ContextMovementStrategy contextMovementStrategy)
        : IExpenseService
    {
        public async Task<bool> CreateExpenses(List<ExpenseDetails> expenseDetails)
        {
            var resultBatch = true;

            foreach (var expense in expenseDetails)
            {
                var result = await CreateExpense(expense);
                if (!result)
                    resultBatch = false;
            }

            return resultBatch;
        }

        public async Task<bool> CreateExpense(ExpenseDetails expenseDetails)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var expense = mapper.Map<Expense>(expenseDetails);

            var movement = new Movement(expenseDetails);

            var strategy = contextMovementStrategy.GetMovementStrategy(movement.PaymentMethod);
            var result = await strategy.Debit(movement);

            if (movement.TargetIncomeId.HasValue)
                expense.GeneratedIncomeId = movement.TargetIncomeId.Value;

            expenseRepository.Create(expense);

            scope.Complete();

            return result;
        }
        
        public async Task<bool> DeleteExpense(int id)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var expense = expenseRepository.GetById(id);
            var expenseDetails = mapper.Map<ExpenseDetails>(expense);

            expenseRepository.Delete(expense);

            var strategy = contextMovementStrategy.GetMovementStrategy((PFM.DataAccessLayer.Enumerations.PaymentMethod)expenseDetails.PaymentMethodId);
            var result = await strategy.Credit(new Movement(expenseDetails));

            scope.Complete();

            return result;
        }

        public Task<ExpenseDetails> GetById(int id)
        {
            var expense = expenseRepository
                            .GetList2(u => u.ExpenseType, u => u.PaymentMethod)
                            .SingleOrDefault(x => x.Id == id);

            return expense == null ? null : Task.FromResult(mapper.Map<ExpenseDetails>(expense));
        }

        public Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            var Expense = expenseRepository.GetById(id);
            Expense.HasBeenAlreadyDebited = debitStatus;
            expenseRepository.Update(Expense);
            return Task.FromResult(true);
        }

        public async Task<IList<ExpenseList>> GetExpenses(PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search)
        {
            var searchParameters = mapper.Map<PFM.DataAccessLayer.SearchParameters.ExpenseGetListSearchParameters>(search);
            var expenses = expenseRepository.GetByParameters(searchParameters).ToList();

            if (!string.IsNullOrEmpty(search.UserId))
            {
                var accountsForUserResponse = await bankAccountApi.GetList(search.UserId);
                var accountsForUser = JsonConvert.DeserializeObject<List<AccountDetails>>(accountsForUserResponse.Data.ToString() ?? string.Empty);
                var filterAccounts = accountsForUser.Select(x => x.Id);

                expenses = expenses.Where(x => filterAccounts.Contains(x.AccountId)).ToList();
            }

            var mappedExpenses = expenses.Select(mapper.Map<ExpenseList>);
            return mappedExpenses.ToList();
        }
    }
}