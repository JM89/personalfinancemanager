using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Threading;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateSavings")]
    public class CreateSavingsSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _countSavings, _countMovements, _countIncomes, _sourceAccountId, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount;

        private readonly IBankAccountService _bankAccountService;
        private readonly ISavingService _savingService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;

        public CreateSavingsSteps()
        {
            _bankAccountService = new BankAccountService();
            _savingService = new SavingService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
        }

        [Given(@"I have accessed the Saving List page")]
        public void GivenIHaveAccessedTheSavingListPage()
        {
            _ctx.GotToUrl("/Saving/Index");

            // Get Source Account Amount Before Creating Savings
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            // Get Number Of Savings Before Creating Savings
            _countSavings = _savingService.CountSavings();

            // Get Number Of Incomes Before Creating Savings
            _countIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Savings
            _countMovements = _historicMovementService.CountMovements();
        }
        
        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            var createBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_create"));
            createBtn.Click();
        }
        
        [When(@"I enter Amount")]
        public void WhenIEnterAmount()
        {
            var amountTxt = _ctx.WebDriver.FindElement(By.Id("Amount"));
            amountTxt.Clear();
            amountTxt.SendKeys("100.00");
        }

        [When(@"I select the first Saving Account")]
        public void WhenISelectTheFirstSavingAccount()
        {
            var accountDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            if (accountDdl.Options.Count < 2)
                throw new Exception("TargetInternalAccountId has no option. At least 1 expected.");
            accountDdl.SelectByIndex(1);

            // Get Target Account Amount Before Creating Savings
            _targetAccountId = Convert.ToInt32(accountDdl.Options[1].GetAttribute("value"));
            _targetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }
        
        [Then(@"the Saving Has Been Created")]
        public void ThenTheSavingHasBeenCreated()
        {
            // Get Number Of Savings After
            var newCountSavings = _savingService.CountSavings();

            Assert.AreEqual(newCountSavings, _countSavings + 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);

            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount + 100);
        }

        [Then(@"an income has been created")]
        public void ThenAnIncomeHasBeenCreated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, _countIncomes + 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
