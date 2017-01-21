using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.RequestObjects;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Services.MovementStrategy;
using System;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly IExpenditureRepository _expenditureRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;

        public ExpenditureService(IExpenditureRepository expenditureRepository, IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository, IIncomeRepository incomeRepository,
            IHistoricMovementRepository historicMovementRepository)
        {
            this._expenditureRepository = expenditureRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._incomeRepository = incomeRepository;
            this._historicMovementRepository = historicMovementRepository;
        }

        public void CreateExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = Mapper.Map<ExpenditureModel>(expenditureEditModel);

            var movement = new Movement(expenditureEditModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Debit();

            if (movement.TargetIncomeId.HasValue)
                expenditureModel.GeneratedIncomeId = movement.TargetIncomeId.Value;

            _expenditureRepository.Create(expenditureModel);
        }
        
        public void EditExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = _expenditureRepository.GetById(expenditureEditModel.Id, true);

            var oldMovement = new Movement(Mapper.Map<ExpenditureEditModel>(expenditureModel));

            expenditureModel = Mapper.Map<ExpenditureModel>(expenditureEditModel);
            if (expenditureModel.GeneratedIncomeId.HasValue)
            {
                expenditureModel.GeneratedIncomeId = (int?)null;
                _expenditureRepository.Update(expenditureModel);
            }

            var strategy = ContextMovementStrategy.GetMovementStrategy(oldMovement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            var newMovement = new Movement(expenditureEditModel);

            strategy.UpdateDebit(newMovement);

            if (newMovement.TargetIncomeId.HasValue)
            {
                // Update the GenerateIncomeId.
                expenditureModel.GeneratedIncomeId = newMovement.TargetIncomeId.Value;
            }

            _expenditureRepository.Update(expenditureModel);
        }

        public void DeleteExpenditure(int id)
        {
            var expenditureModel = _expenditureRepository.GetById(id);
            var expenditureEditModel = Mapper.Map<ExpenditureEditModel>(expenditureModel);

            _expenditureRepository.Delete(expenditureModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(expenditureEditModel), _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Credit();
        }

        public ExpenditureEditModel GetById(int id)
        {
            var expenditure = _expenditureRepository.GetList()
                                    .Include(u => u.Account.Currency)
                                    .Include(u => u.TypeExpenditure)
                                    .Include(u => u.PaymentMethod).SingleOrDefault(x => x.Id == id);

            if (expenditure == null)
            {
                return null;
            }

            return Mapper.Map<ExpenditureEditModel>(expenditure);
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            var expenditure = _expenditureRepository.GetById(id);
            expenditure.HasBeenAlreadyDebited = debitStatus;
            _expenditureRepository.Update(expenditure);
        }

        public IList<ExpenditureListModel> GetExpenditures(ExpenditureSearch search)
        {
            var expenditures = _expenditureRepository.GetList()
                .Where(x =>
                    (!search.AccountId.HasValue || (search.AccountId.HasValue && x.AccountId == search.AccountId.Value))
                    && (!search.StartDate.HasValue || (search.StartDate.HasValue && x.DateExpenditure >= search.StartDate))
                    && (!search.EndDate.HasValue || (search.EndDate.HasValue && x.DateExpenditure < search.EndDate))
                    && (!search.ExpenditureTypeId.HasValue || (search.ExpenditureTypeId.HasValue && x.TypeExpenditureId == search.ExpenditureTypeId.Value)))
                .Include(u => u.Account.Currency)
                .Include(u => u.TypeExpenditure)
                .Include(u => u.PaymentMethod).ToList();

            var mappedExpenditures = expenditures.Select(x => Mapper.Map<ExpenditureListModel>(x));

            return mappedExpenditures.ToList();
        }
    }
}