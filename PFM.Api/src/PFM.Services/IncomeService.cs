using AutoMapper;
using PFM.Api.Contracts.Income;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Interfaces;
using PFM.Services.MovementStrategy;
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
        private readonly ContextMovementStrategy _contextMovementStrategy;

        public IncomeService(IIncomeRepository incomeRepository, IBankAccountCache bankAccountCache, ContextMovementStrategy contextMovementStrategy)
        {
            this._incomeRepository = incomeRepository;
            this._bankAccountCache = bankAccountCache;
            this._contextMovementStrategy = contextMovementStrategy;
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

                var movement = new Movement(incomeDetails);

                var strategy = _contextMovementStrategy.GetMovementStrategy(DataAccessLayer.Enumerations.PaymentMethod.Transfer);
                var result = await strategy.Credit(movement);

                _incomeRepository.Create(income);

                scope.Complete();

                return result;
            }
        }

        public async Task<IList<IncomeList>> GetIncomes(int accountId)
        {
            var incomes = _incomeRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

            var mappedIncomes = new List<IncomeList>();
            
            foreach (var income in incomes)
            {
                var map = Mapper.Map<IncomeList>(income);

                var account = await _bankAccountCache.GetById(income.AccountId);
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
                var incomeDetails = Mapper.Map<IncomeDetails>(income);

                var strategy = _contextMovementStrategy.GetMovementStrategy(DataAccessLayer.Enumerations.PaymentMethod.Transfer);
                var result = await strategy.Debit(new Movement(incomeDetails));

                _incomeRepository.Delete(income);

                scope.Complete();

                return result;
            }
        }
    }
}