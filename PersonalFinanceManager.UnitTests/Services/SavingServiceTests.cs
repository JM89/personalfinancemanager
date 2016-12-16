using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Models.Saving;
using AutoMapper;
using PersonalFinanceManager.Utils.Automapper;
using PersonalFinanceManager.Entities;
using System.Data.Entity;
using System.Collections.Generic;
using PersonalFinanceManager.DataAccess;
using System.Linq;
using System.Collections.ObjectModel;

namespace PersonalFinanceManager.UnitTests
{
    [TestClass]
    public class SavingServiceTests
    {
        private Mock<ISavingRepository> mockSavingRepository;
        private Mock<IBankAccountRepository> mockBankAccountRepository;
        private Mock<IHistoricMovementRepository> mockHistoricMovementRepository;
        private Mock<IIncomeRepository> mockIncomeRepository;
        private ISavingService savingService;
        private Mock<DbSet<SavingModel>> savingDbSet;

        [TestInitialize]
        public void Setup()
        {
            mockSavingRepository = new Mock<ISavingRepository>();
            
            mockBankAccountRepository = new Mock<IBankAccountRepository>();
            mockHistoricMovementRepository = new Mock<IHistoricMovementRepository>();
            mockIncomeRepository = new Mock<IIncomeRepository>();

            savingService = new SavingService(
                                mockSavingRepository.Object, 
                                mockBankAccountRepository.Object,
                                mockHistoricMovementRepository.Object, 
                                mockIncomeRepository.Object);

            TestHelper.ConfigureAutomapper();

            var data = new List<SavingModel> { new SavingModel() { Id = 1 } };
            savingDbSet = TestHelper.GetDbSet(data);
        }

        [TestMethod]
        public void Saving_Should_Be_Added()
        {
            var savingEditModel = new SavingEditModel()
            {
                AccountId = 1,
                DateSaving = DateTime.Today,
                TargetInternalAccountId = 2
            };

            savingService.CreateSaving(savingEditModel);

            mockSavingRepository.Verify(x => x.Create(It.Is<SavingModel>(t => t.AccountId == savingEditModel.AccountId
                                                                        && t.DateSaving == savingEditModel.DateSaving
                                                                        && t.TargetInternalAccountId == savingEditModel.TargetInternalAccountId)));
        }

        [TestMethod]
        public void Saving_Should_Be_Edited()
        {
            mockSavingRepository.Setup(t => t.GetList()).Returns(savingDbSet.Object);
            mockSavingRepository.Setup(t => t.GetList().AsNoTracking()).Returns(savingDbSet.Object);

            var savingEditModel = new SavingEditModel()
            {
                Id = 1,
                AccountId = 1,
                DateSaving = DateTime.Today,
                TargetInternalAccountId = 2
            };

            savingService.EditSaving(savingEditModel);

            mockSavingRepository.Verify(x => x.Update(It.Is<SavingModel>(t => t.Id == savingEditModel.Id 
                                                                        && t.AccountId == savingEditModel.AccountId
                                                                        && t.DateSaving == savingEditModel.DateSaving
                                                                        && t.TargetInternalAccountId == savingEditModel.TargetInternalAccountId)));
        }

        [TestMethod]
        public void Saving_Should_Be_Deleted()
        {
            int savingId = 1;

            mockSavingRepository.Setup(t => t.GetById(savingId)).Returns(savingDbSet.Object.First());

            savingService.DeleteSaving(savingId);

            mockSavingRepository.Verify(x => x.Delete(It.Is<SavingModel>(t => t.Id == savingId)));
        }
    }
}
