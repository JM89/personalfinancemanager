using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

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

            var accountModel = _bankAccountRepository.GetById(incomeModel.AccountId);
            //MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, accountModel, incomeModel.Cost, MovementType.Income);
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
                //MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, account, oldCost, MovementType.Income);
                //MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, account, income.Cost, MovementType.Income);
            }
        }

        public void DeleteIncome(int id)
        {
            var incomeModel = _incomeRepository.GetById(id);
            _incomeRepository.Delete(incomeModel);

            var accountModel = _bankAccountRepository.GetById(incomeModel.AccountId);
            //MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, accountModel, incomeModel.Cost, MovementType.Income);
        }
    }
}