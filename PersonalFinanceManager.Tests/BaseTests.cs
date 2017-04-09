using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Services;
using System.Data.Entity;
using PersonalFinanceManager.Core.Automapper;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonalFinanceManager.Entities.SearchParameters;

namespace PersonalFinanceManager.Tests
{
    public class BaseTests
    {
        protected Mock<IExpenditureRepository> MockExpenditureRepository;
        protected Mock<IBankAccountRepository> MockBankAccountRepository;
        protected Mock<IAtmWithdrawRepository> MockAtmWithdrawRepository;
        protected Mock<IIncomeRepository> MockIncomeRepository;
        protected Mock<IHistoricMovementRepository> MockHistoricMovementRepository;
        protected Mock<IExpenditureTypeRepository> MockExpenditureTypeRepository;
        protected Mock<ISavingRepository> MockSavingRepository;

        public BaseTests()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
                cfg.AddProfile<EntityToEntityMapping>();
                cfg.AddProfile<SearchParametersMapping>();
            });
        }

        protected ExpenditureService SetupExpenditureService(AccountModel account, List<ExpenditureTypeModel> types, List<ExpenditureModel> expenses,
            List<IncomeModel> incomes, List<SavingModel> savings)
        {
            MockExpenditureRepository = new Mock<IExpenditureRepository>();
            MockExpenditureRepository
                .Setup(x => x.GetByParameters(It.IsAny<ExpenditureGetListSearchParameters>()))
                .Returns(expenses);

            MockBankAccountRepository = new Mock<IBankAccountRepository>();
            MockBankAccountRepository
                .Setup(x => x.GetById(account.Id, y => y.Bank, y => y.Currency))
                .Returns(account);

            MockAtmWithdrawRepository = new Mock<IAtmWithdrawRepository>();

            MockIncomeRepository = new Mock<IIncomeRepository>();
            MockIncomeRepository
                .Setup(x => x.GetList())
                .ReturnsDbSet(incomes);

            MockHistoricMovementRepository = new Mock<IHistoricMovementRepository>();

            MockExpenditureTypeRepository = new Mock<IExpenditureTypeRepository>();
            MockExpenditureTypeRepository
                .Setup(x => x.GetList2())
                .Returns(types);

            MockSavingRepository = new Mock<ISavingRepository>();
            MockSavingRepository
                .Setup(x => x.GetList())
                .ReturnsDbSet(savings);

            var service = new ExpenditureService(
                MockExpenditureRepository.Object,
                MockBankAccountRepository.Object,
                MockAtmWithdrawRepository.Object,
                MockIncomeRepository.Object,
                MockHistoricMovementRepository.Object,
                MockExpenditureTypeRepository.Object,
                MockSavingRepository.Object);
            return service;
        }
    }
}
