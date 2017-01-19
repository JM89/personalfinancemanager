using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateSavings")]
    public class CreateSavingsSteps
    {
        public IntegrationTestContext ctx = new IntegrationTestContext();
        public int CountSavings, CountMovements, CountIncomes;
        public int SourceAccountId, TargetAccountId;
        public decimal SourceAccountAmount, TargetAccountAmount;

        public IBankAccountService _bankAccountService;
        public ISavingService _savingService;
        public IIncomeService _incomeService;
        public IHistoricMovementService _historicMovementService;

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
            ctx.GotToUrl("/Saving/Index");

            // Get Source Account Amount Before Creating Savings
            SourceAccountId = ctx.SelectedSourceAccountId();
            SourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            // Get Number Of Savings Before Creating Savings
            CountSavings = _savingService.CountSavings();

            // Get Number Of Incomes Before Creating Savings
            CountIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Savings
            CountMovements = _historicMovementService.CountMovements();
        }
        
        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            var createBtn = ctx.WebDriver.FindElement(By.ClassName("btn_create"));
            createBtn.Click();
        }
        
        [When(@"I enter Amount")]
        public void WhenIEnterAmount()
        {
            var amountTxt = ctx.WebDriver.FindElement(By.Id("Amount"));
            amountTxt.Clear();
            amountTxt.SendKeys("100.00");
        }

        [When(@"I select the first Saving Account")]
        public void WhenISelectTheFirstSavingAccount()
        {
            var accountDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            if (accountDdl.Options.Count < 2)
                throw new Exception("TargetInternalAccountId has no option. At least 1 expected.");
            accountDdl.SelectByIndex(1);

            // Get Target Account Amount Before Creating Savings
            TargetAccountId = Convert.ToInt32(accountDdl.Options[1].GetAttribute("value"));
            TargetAccountAmount = _bankAccountService.GetAccountAmount(TargetAccountId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();
        }
        
        [Then(@"the Saving Has Been Created")]
        public void ThenTheSavingHasBeenCreated()
        {
            // Get Number Of Savings After
            var newCountSavings = _savingService.CountSavings();

            Assert.AreEqual(newCountSavings, CountSavings + 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, SourceAccountAmount - 100);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(TargetAccountId);

            Assert.AreEqual(newTargetAccountAmount, TargetAccountAmount + 100);
        }

        [Then(@"an income has been created")]
        public void ThenAnIncomeHasBeenCreated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, CountIncomes + 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, CountMovements + 1);
        }
    }
}
