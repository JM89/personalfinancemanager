using AutoMapper;
using PFM.Api.Contracts.Income;
using PFM.Bank.Event.Contracts;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
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
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IEventPublisher _eventPublisher;

        private readonly string OperationType = "Income";

        public IncomeService(IIncomeRepository incomeRepository, IBankAccountRepository bankAccountRepository,IEventPublisher eventPublisher)
        {
            this._incomeRepository = incomeRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._eventPublisher = eventPublisher;
        }

        public Task<bool> CreateIncomes(List<IncomeDetails> incomeDetails)
        {
            var resultBatch = true;
            incomeDetails.ForEach(async (income) => {
                var result = await CreateIncome(income);
                if (!result)
                    resultBatch = false;
            });
            return Task.FromResult(resultBatch); 
        }

        public async Task<bool> CreateIncome(IncomeDetails incomeDetails)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var income = Mapper.Map<Income>(incomeDetails);
                _incomeRepository.Create(income);

                var account = _bankAccountRepository.GetById(income.AccountId, a => a.Currency, a => a.Bank);

                var evt = new BankAccountCredited()
                {
                    Id = account.Id,
                    BankId = account.Bank.Id,
                    CurrencyId = account.Currency.Id,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance + incomeDetails.Cost,
                    UserId = account.User_Id,
                    OperationDate = income.DateIncome,
                    OperationType = OperationType
                };

                account.CurrentBalance += incomeDetails.Cost;
                _bankAccountRepository.Update(account);

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }

        public IList<IncomeList> GetIncomes(int accountId)
        {
            var incomes = _incomeRepository.GetList2(u => u.Account.Currency).Where(x => x.AccountId == accountId).ToList();

            var mappedIncomes = incomes.Select(x => Mapper.Map<IncomeList>(x));
            
            return mappedIncomes.ToList();
        }

        public IncomeDetails GetById(int id)
        {
            var income = _incomeRepository.GetById(id);

            if (income == null)
            {
                return null;
            }

            return Mapper.Map<IncomeDetails>(income);
        }

        public async Task<bool> DeleteIncome(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var income = _incomeRepository.GetById(id);

                var account = _bankAccountRepository.GetById(income.AccountId, a => a.Currency, a => a.Bank);

                var evt = new BankAccountDebited()
                {
                    Id = account.Id,
                    BankId = account.Bank.Id,
                    CurrencyId = account.Currency.Id,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance - income.Cost,
                    UserId = account.User_Id,
                    OperationDate = income.DateIncome,
                    OperationType = OperationType
                };

                account.CurrentBalance -= income.Cost;
                _bankAccountRepository.Update(account);

                _incomeRepository.Delete(income);

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }
    }
}