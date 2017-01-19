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

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(expenditureEditModel), _bankAccountRepository, _historicMovementRepository, _incomeRepository);
            strategy.Debit();
        }
        
        public void EditExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = _expenditureRepository.GetList().AsNoTracking().SingleOrDefault(x => x.Id == expenditureEditModel.Id);
            var currentSavingEditModel = Mapper.Map<ExpenditureEditModel>(expenditureModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(currentSavingEditModel), _bankAccountRepository, _historicMovementRepository, _incomeRepository);

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
            
            strategy.UpdateDebit(new Movement(expenditureEditModel));
        }

        public void DeleteExpenditure(int id)
        {
            var expenditureModel = _expenditureRepository.GetById(id);
            var expenditureEditModel = Mapper.Map<ExpenditureEditModel>(expenditureModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(expenditureEditModel), _bankAccountRepository, _historicMovementRepository, _incomeRepository);
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