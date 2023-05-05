using AutoMapper;
using PFM.Api.Contracts.Expense;
using PFM.Api.Contracts.Income;
using PFM.Bank.Event.Contracts;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events.Interfaces;
using PFM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PFM.Services
{
    public class IncomeService: IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IBankAccountCache _bankAccountCache;
        private readonly IEventPublisher _eventPublisher;

        private readonly string OperationType = "Income";

        public IncomeService(IIncomeRepository incomeRepository, IBankAccountCache bankAccountCache,IEventPublisher eventPublisher)
        {
            this._incomeRepository = incomeRepository;
            this._bankAccountCache = bankAccountCache;
            this._eventPublisher = eventPublisher;
        }

        public async Task<bool> CreateIncomes(List<IncomeDetails> incomeDetails)
        {
            var resultBatch = true;

            foreach (var income in incomeDetails)
            {
                var result = await CreateIncome(income);
                if (!result)
                    resultBatch = false;
            }

            return resultBatch; 
        }

        public async Task<bool> CreateIncome(IncomeDetails incomeDetails)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var income = Mapper.Map<Income>(incomeDetails);
                _incomeRepository.Create(income);

                var account = await _bankAccountCache.GetById(income.AccountId);

                var evt = new BankAccountCredited()
                {
                    Id = account.Id,
                    BankId = account.BankId,
                    CurrencyId = account.CurrencyId,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance + incomeDetails.Cost,
                    UserId = account.OwnerId,
                    OperationDate = income.DateIncome,
                    OperationType = OperationType
                };

                account.CurrentBalance += incomeDetails.Cost;

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }

        public async Task<IList<IncomeList>> GetIncomes(int accountId)
        {
            var incomes = _incomeRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

            var mappedIncomes = new List<IncomeList>();
            
            foreach (var income in incomes)
            {
                var map = Mapper.Map<IncomeList>(income);

                var account = await _bankAccountCache.GetById(income.Id);
                map.AccountCurrencySymbol = account.CurrencySymbol;

                mappedIncomes.Add(map);
            }

            return mappedIncomes.ToList();
        }

        public Task<IncomeDetails> GetById(int id)
        {
            var income = _incomeRepository.GetById(id);

            if (income == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<IncomeDetails>(income));
        }

        public async Task<bool> DeleteIncome(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var income = _incomeRepository.GetById(id);

                var account = await _bankAccountCache.GetById(income.AccountId);

                var evt = new BankAccountDebited()
                {
                    Id = account.Id,
                    BankId = account.BankId,
                    CurrencyId = account.CurrencyId,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance - income.Cost,
                    UserId = account.OwnerId,
                    OperationDate = income.DateIncome,
                    OperationType = OperationType
                };

                account.CurrentBalance -= income.Cost;

                _incomeRepository.Delete(income);

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }
    }
}