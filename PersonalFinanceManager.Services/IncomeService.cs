using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Services.Helpers;
using PersonalFinanceManager.Entities.Enumerations;

namespace PersonalFinanceManager.Services
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
        
        public void CreateIncome(IncomeEditModel incomeEditModel)
        {
            var incomeModel = Mapper.Map<IncomeModel>(incomeEditModel);
            _incomeRepository.Create(incomeModel);

            var account = _bankAccountRepository.GetById(incomeModel.AccountId);
            MovementHelpers.Credit(_historicMovementRepository, incomeModel.Cost, account.Id, ObjectType.Account, account.CurrentBalance);

            account.CurrentBalance += incomeEditModel.Cost;
            _bankAccountRepository.Update(account);
        }

        public IList<IncomeListModel> GetIncomes(int accountId)
        {
            var incomes = _incomeRepository.GetList().Include(u => u.Account.Currency).Where(x => x.AccountId == accountId).ToList();

            var incomesModel = incomes.Select(x => Mapper.Map<IncomeListModel>(x));
            
            return incomesModel.ToList();
        }

        public IncomeEditModel GetById(int id)
        {
            var income = _incomeRepository.GetById(id);

            if (income == null)
            {
                return null;
            }

            return Mapper.Map<IncomeEditModel>(income);
        }

        public void EditIncome(IncomeEditModel incomeEditModel)
        {
            var income = _incomeRepository.GetById(incomeEditModel.Id);

            var oldCost = income.Cost;

            income.Description = incomeEditModel.Description;
            income.Cost = incomeEditModel.Cost;
            income.AccountId = incomeEditModel.AccountId;
            income.DateIncome = incomeEditModel.DateIncome;
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
            var incomeModel = _incomeRepository.GetById(id);

            var account = _bankAccountRepository.GetById(incomeModel.AccountId);
            MovementHelpers.Debit(_historicMovementRepository, incomeModel.Cost, account.Id, ObjectType.Account, account.CurrentBalance);
            account.CurrentBalance -= incomeModel.Cost;
            _bankAccountRepository.Update(account);

            _incomeRepository.Delete(incomeModel);
        }
    }
}