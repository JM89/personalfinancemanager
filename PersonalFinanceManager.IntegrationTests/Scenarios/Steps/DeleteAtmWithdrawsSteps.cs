using PersonalFinanceManager.IntegrationTests.Infrastructure;
using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Scenarios.PreActions;
using PersonalFinanceManager.ServicesForTests;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding]
    public class DeleteAtmWithdrawsSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _sourceAccountId, _countAtmWithdraws, _countMovements;
        private decimal _sourceAccountAmount;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly IAtmWithdrawService _atmWithdrawService;
        private readonly IHistoricMovementService _historicMovementService;

        public DeleteAtmWithdrawsSteps()
        {
            _bankAccountService = new BankAccountService();
            _atmWithdrawService = new AtmWithdrawService();
            _historicMovementService = new HistoricMovementService();
        }

        [BeforeScenario]
        public void PrepareForTest()
        {
            CreateAtmWithdraws.Execute(_ctx);
        }

        [Given(@"I have accessed the ATM Withdraw List page")]
        public void GivenIHaveAccessedTheAtmWithdrawListPage()
        {
            // Get Source Account Amount Before Creating AtmWithdraws
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            _ctx.GotToUrl("/AtmWithdraw/Index");

            // Get Number Of Incomes Before Creating AtmWithdraws
            _countAtmWithdraws = _atmWithdrawService.CountAtmWithdraws();

            // Get Number Of Movements Before Creating AtmWithdraws
            _countMovements = _historicMovementService.CountMovements();
        }

        [Given(@"I have at least one ATM Withdraw in the list")]
        public void GivenIHaveAtLeastOneAtmWithdrawInTheList()
        {
            var atmWithdraws = _ctx.WebDriver.FindElements(By.ClassName("trAtmWithdraw"));
            if (atmWithdraws.Count < 1)
            {
                throw new Exception("There is no atm withdraws to delete");
            }
            _firstRow = atmWithdraws[0];
        }
        
        [When(@"I click on delete for the first ATM Withdraw")]
        public void WhenIClickOnDeleteForTheFirstAtmWithdraw()
        {
            var deleteConfirmBtn = _firstRow.FindElement(By.ClassName("btn_delete"));
            deleteConfirmBtn.Click();
        }
        
        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            var deleteAtmWithdrawPage = _ctx.WebDriver.FindElement(By.TagName("h5"));
            if (deleteAtmWithdrawPage.Text != "Delete an ATM withdraw")
            {
                throw new Exception("The confirmation of deletion should be there.");
            }
            var deleteBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_delete"));
            deleteBtn.Click();
            
            Thread.Sleep(2000);
        }
        
        [Then(@"the ATM Withdraw has been removed")]
        public void ThenTheAtmWithdrawHasBeenRemoved()
        {
            // Get Number Of ATM Withdraws After
            var newCountAtmWithdraws = _atmWithdrawService.CountAtmWithdraws();

            Assert.AreEqual(newCountAtmWithdraws, _countAtmWithdraws - 1);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + 100);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }

        [AfterScenario]
        public void TestTearDown()
        {
            _ctx.StopTest();
        }
    }
}
