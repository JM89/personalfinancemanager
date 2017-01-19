using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteSavings")]
    public class DeleteSavingsSteps
    {
        public IntegrationTestContext ctx = new IntegrationTestContext();
        public int CountSavings, CountMovements, CountIncomes;
        public int SourceAccountId, TargetAccountId;
        public decimal SourceAccountAmount, TargetAccountAmount;
        public decimal CostSaving;

        public IWebElement FirstRow;

        public IBankAccountService _bankAccountService;
        public ISavingService _savingService;
        public IIncomeService _incomeService;
        public IHistoricMovementService _historicMovementService;

        public DeleteSavingsSteps()
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
        
        [Given(@"I have at least one saving in the list")]
        public void GivenIHaveAtLeastOneSavingInTheList()
        {
            FirstRow = ctx.WebDriver.FindElement(By.Id("row-1"));
            if (FirstRow == null)
            {
                throw new Exception("There is no saving to delete");
            }
        }
        
        [When(@"I click on delete for the first saving")]
        public void WhenIClickOnDeleteForTheFirstSaving()
        {
            var costValue = FirstRow.FindElement(By.ClassName("tdAmount"));
            CostSaving = Convert.ToDecimal(costValue.Text.Substring(1));

            var targetAccountHid = FirstRow.FindElement(By.Id("item_TargetInternalAccountId"));
            TargetAccountId = Convert.ToInt32(targetAccountHid.GetAttribute("value"));
            TargetAccountAmount = _bankAccountService.GetAccountAmount(TargetAccountId);

            var deleteConfirmBtn = FirstRow.FindElement(By.ClassName("btn_delete"));
            deleteConfirmBtn.Click();
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            var deleteSavingPage = ctx.WebDriver.FindElement(By.TagName("h5"));
            if (deleteSavingPage.Text != "Delete saving")
            {
                throw new Exception("The confirmation of deletion should be there.");
            }
            var deleteBtn = ctx.WebDriver.FindElement(By.ClassName("btn_delete"));
            deleteBtn.Click();
        }

        [Then(@"the Saving has been removed")]
        public void ThenTheSavingHasBeenRemoved()
        {
            // Get Number Of Savings After
            var newCountSavings = _savingService.CountSavings();

            Assert.AreEqual(newCountSavings, CountSavings - 1);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, SourceAccountAmount + CostSaving);
        }
        
        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(TargetAccountId);

            Assert.AreEqual(newTargetAccountAmount, TargetAccountAmount - CostSaving);
        }
        
        [Then(@"an income has been removed")]
        public void ThenAnIncomeHasBeenRemoved()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, CountIncomes - 1);
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
