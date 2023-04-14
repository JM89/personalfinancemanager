using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.Entities;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Helpers;
using PFM.DataAccessLayer.Enumerations;
using PFM.Services.DTOs.Income;

namespace PFM.Services
{
    public class IncomeService: IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;

        public IncomeService(IIncomeRepository incomeRepository, IBankAccountRepository bankAccountRepository,
            IHistoricMovementRepository historicMovementRepository)
        {
            this._incomeRepository = incomeRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._historicMovementRepository = historicMovementRepository;
        }

        public void CreateIncomes(List<IncomeDetails> incomeDetails)
        {
            incomeDetails.ForEach(CreateIncome);
        }

        public void CreateIncome(IncomeDetails incomeDetails)
        {
            var income = Mapper.Map<Income>(incomeDetails);
            _incomeRepository.Create(income);

            var account = _bankAccountRepository.GetById(income.AccountId);
            MovementHelpers.Credit(_historicMovementRepository, income.Cost, account.Id, ObjectType.Account, account.CurrentBalance);

            account.CurrentBalance += incomeDetails.Cost;
            _bankAccountRepository.Update(account);
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

        public void EditIncome(IncomeDetails incomeDetails)
        {
            var income = _incomeRepository.GetById(incomeDetails.Id);

            var oldCost = income.Cost;

            income.Description = incomeDetails.Description;
            income.Cost = incomeDetails.Cost;
            income.AccountId = incomeDetails.AccountId;
            income.DateIncome = incomeDetails.DateIncome;
            _incomeRepository.Update(income);

            if (oldCost != income.Cost)
            {
                var account = _bankAccountRepository.GetById(income.AccountId);
                MovementHelpers.Debit(_historicMovementRepository, oldCost, account.Id, ObjectType.Account, account.CurrentBalance);
                account.CurrentBalance -= oldCost;
                MovementHelpers.Credit(_historicMovementRepository, income.Cost, account.Id, ObjectType.Account, account.CurrentBalance);
                account.CurrentBalance += income.Cost;
                _bankAccountRepository.Update(account);
            }
        }

        public void DeleteIncome(int id)
        {
            var income = _incomeRepository.GetById(id);

            var account = _bankAccountRepository.GetById(income.AccountId);
            MovementHelpers.Debit(_historicMovementRepository, income.Cost, account.Id, ObjectType.Account, account.CurrentBalance);
            account.CurrentBalance -= income.Cost;
            _bankAccountRepository.Update(account);

            _incomeRepository.Delete(income);
        }
    }
}