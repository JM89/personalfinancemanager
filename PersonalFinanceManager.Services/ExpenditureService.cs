using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.ExpenditureStrategy;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.RequestObjects;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly IExpenditureRepository _expenditureRepository;
        protected IBankAccountRepository _bankAccountRepository;
        protected IAtmWithdrawRepository _atmWithdrawRepository;
        protected IIncomeRepository _incomeRepository;
        protected IHistoricMovementRepository _historicMovementRepository;

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
            _expenditureRepository.Create(expenditureModel);

            var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_bankAccountRepository, _atmWithdrawRepository, _incomeRepository, _historicMovementRepository, expenditureModel);

            strategy.Debit();
        }
        
        public void EditExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = _expenditureRepository.GetById(expenditureEditModel.Id);

            var oldExpenditureModel = new ExpenditureModel() {
                AccountId = expenditureModel.AccountId, 
                Cost = expenditureModel.Cost,
                AtmWithdrawId = expenditureModel.AtmWithdrawId,
                DateExpenditure = expenditureModel.DateExpenditure,
                Description = expenditureModel.Description, 
                PaymentMethodId = expenditureModel.PaymentMethodId,
                TargetInternalAccountId = expenditureModel.TargetInternalAccountId
            };
            
            var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_bankAccountRepository, _atmWithdrawRepository, _incomeRepository, _historicMovementRepository, oldExpenditureModel);

            expenditureModel.DateExpenditure = expenditureEditModel.DateExpenditure;
            expenditureModel.Description = expenditureEditModel.Description;
            expenditureModel.AccountId = expenditureEditModel.AccountId;
            expenditureModel.PaymentMethodId = expenditureEditModel.PaymentMethodId;
            expenditureModel.TypeExpenditureId = expenditureEditModel.TypeExpenditureId;
            expenditureModel.Cost = expenditureEditModel.Cost;
            expenditureModel.HasBeenAlreadyDebited = expenditureEditModel.HasBeenAlreadyDebited;
            expenditureModel.AtmWithdrawId = expenditureEditModel.AtmWithdrawId;
            expenditureModel.TargetInternalAccountId = expenditureEditModel.TargetInternalAccountId;

            _expenditureRepository.Update(expenditureModel);
            
            strategy.UpdateDebit(expenditureModel);
        }

        public void DeleteExpenditure(int id)
        {
            var expenditureModel = _expenditureRepository.GetById(id);

            var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_bankAccountRepository, _atmWithdrawRepository, _incomeRepository, _historicMovementRepository, expenditureModel);

            strategy.Credit();

            _expenditureRepository.Delete(expenditureModel);
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