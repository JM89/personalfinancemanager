using AutoMapper;
using Moq;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.SearchParameters;
using PFM.Services.Events.Interfaces;
using PFM.Services.Interfaces.Services;
using System.Collections.Generic;

namespace PFM.UnitTests
{
    public class BaseTests
    {
        protected Mock<IExpenseRepository> MockExpenseRepository;
        protected Mock<IBankAccountRepository> MockBankAccountRepository;
        protected Mock<IAtmWithdrawRepository> MockAtmWithdrawRepository;
        protected Mock<IIncomeRepository> MockIncomeRepository;
        protected Mock<IHistoricMovementRepository> MockHistoricMovementRepository;
        protected Mock<IExpenseTypeRepository> MockExpenseTypeRepository;
        protected Mock<ISavingRepository> MockSavingRepository;
        protected Mock<IEventPublisher> MockEventPublisher;

        public BaseTests()
        {
           
        }

        protected ExpenseService SetupExpenseService(Account account, List<ExpenseType> types, List<Expense> expenses,
            List<Income> incomes, List<Saving> savings)
        {
            MockExpenseRepository = new Mock<IExpenseRepository>();
            MockExpenseRepository
                .Setup(x => x.GetByParameters(It.IsAny<ExpenseGetListSearchParameters>()))
                .Returns(expenses);

            MockBankAccountRepository = new Mock<IBankAccountRepository>();
            MockBankAccountRepository
                .Setup(x => x.GetById(account.Id, y => y.Bank, y => y.Currency))
                .Returns(account);

            MockAtmWithdrawRepository = new Mock<IAtmWithdrawRepository>();

            MockIncomeRepository = new Mock<IIncomeRepository>();
            MockIncomeRepository.Setup(x => x.GetList2()).Returns(incomes);

            MockHistoricMovementRepository = new Mock<IHistoricMovementRepository>();

            MockExpenseTypeRepository = new Mock<IExpenseTypeRepository>();
            MockExpenseTypeRepository
                .Setup(x => x.GetList2())
                .Returns(types);

            MockSavingRepository = new Mock<ISavingRepository>();
            MockSavingRepository.Setup(x => x.GetList2()).Returns(savings);

            MockEventPublisher = new Mock<IEventPublisher>();
            MockEventPublisher.Setup(x => x.PublishAsync(It.IsAny<IEvent>(), default)).ReturnsAsync(true);

            var service = new ExpenseService(
                MockExpenseRepository.Object,
                MockBankAccountRepository.Object,
                MockAtmWithdrawRepository.Object,
                MockIncomeRepository.Object,
                MockHistoricMovementRepository.Object,
                MockExpenseTypeRepository.Object,
                MockSavingRepository.Object,
                MockEventPublisher.Object);
            return service;
        }
    }
}
