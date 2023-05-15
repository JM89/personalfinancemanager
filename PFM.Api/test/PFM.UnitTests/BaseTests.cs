﻿using Moq;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Event.Contracts.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.SearchParameters;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events.Interfaces;
using PFM.Services.Interfaces.Services;
using System.Collections.Generic;

namespace PFM.UnitTests
{
    public class BaseTests
    {
        protected Mock<IExpenseRepository> MockExpenseRepository;
        protected Mock<IAtmWithdrawRepository> MockAtmWithdrawRepository;
        protected Mock<IIncomeRepository> MockIncomeRepository;
        protected Mock<IExpenseTypeRepository> MockExpenseTypeRepository;
        protected Mock<ISavingRepository> MockSavingRepository;
        protected Mock<IEventPublisher> MockEventPublisher;
        protected Mock<IBankAccountCache> MockBankAccountCache;

        public BaseTests()
        {
           
        }

        protected MovementSummaryService SetupExpenseService(AccountDetails account, List<ExpenseType> types, List<Expense> expenses,
            List<Income> incomes, List<Saving> savings)
        {
            MockExpenseRepository = new Mock<IExpenseRepository>();
            MockExpenseRepository
                .Setup(x => x.GetByParameters(It.IsAny<ExpenseGetListSearchParameters>()))
                .Returns(expenses);

            MockAtmWithdrawRepository = new Mock<IAtmWithdrawRepository>();

            MockIncomeRepository = new Mock<IIncomeRepository>();
            MockIncomeRepository.Setup(x => x.GetList2()).Returns(incomes);

            MockExpenseTypeRepository = new Mock<IExpenseTypeRepository>();
            MockExpenseTypeRepository
                .Setup(x => x.GetList2())
                .Returns(types);

            MockSavingRepository = new Mock<ISavingRepository>();
            MockSavingRepository.Setup(x => x.GetList2()).Returns(savings);

            MockEventPublisher = new Mock<IEventPublisher>();
            MockEventPublisher.Setup(x => x.PublishAsync(It.IsAny<IEvent>(), default)).ReturnsAsync(true);

            MockBankAccountCache = new Mock<IBankAccountCache>();
            MockBankAccountCache.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Bank.Api.Contracts.Account.AccountDetails()
            {
                Id = account.Id,
                BankId = account.BankId,
                CurrencyId = account.CurrencyId,
                Name = account.Name,
                CurrentBalance = account.CurrentBalance,
                InitialBalance = account.InitialBalance
            });

            var service = new MovementSummaryService(
                MockExpenseRepository.Object,
                MockIncomeRepository.Object,
                MockExpenseTypeRepository.Object,
                MockSavingRepository.Object,
                MockBankAccountCache.Object);

            return service;
        }
    }
}
