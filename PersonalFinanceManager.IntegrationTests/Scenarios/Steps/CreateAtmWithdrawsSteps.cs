using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateAtmWithdraws")]
    public class CreateAtmWithdrawsSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _sourceAccountId, _countAtmWithdraws, _countMovements;
        private decimal _sourceAccountAmount;

        private readonly IBankAccountService _bankAccountService;
        private readonly IAtmWithdrawService _atmWithdrawService;
        private readonly IHistoricMovementService _historicMovementService;

        public CreateAtmWithdrawsSteps()
        {
            _bankAccountService = new BankAccountService();
            _atmWithdrawService = new AtmWithdrawService();
            _historicMovementService = new HistoricMovementService();
        }

        [Given(@"I have accessed the ATM Withdraw List page")]
        public void GivenIHaveAccessedTheAtmWithdrawListPage()
        {
            _ctx.GotToUrl("/AtmWithdraw/Index");

            // Get Source Account Amount Before Creating AtmWithdraws
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            // Get Number Of Incomes Before Creating AtmWithdraws
            _countAtmWithdraws = _atmWithdrawService.CountAtmWithdraws();

            // Get Number Of Movements Before Creating AtmWithdraws
            _countMovements = _historicMovementService.CountMovements();
        }
        
        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            var createBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_create"));
            createBtn.Click();
        }
        
        [When(@"I enter Cost")]
        public void WhenIEnterCost()
        {
            var initialAmountTxt = _ctx.WebDriver.FindElement(By.Id("InitialAmount"));
            initialAmountTxt.Clear();
            initialAmountTxt.SendKeys("100.00");
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
        }
        
        [Then(@"an ATM withdraw has been created")]
        public void ThenAnAtmWithdrawHasBeenCreated()
        {
            // Get Number Of ATM Withdraws After
            var newCountAtmWithdraws = _atmWithdrawService.CountAtmWithdraws();

            Assert.AreEqual(newCountAtmWithdraws, _countAtmWithdraws + 1);
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
