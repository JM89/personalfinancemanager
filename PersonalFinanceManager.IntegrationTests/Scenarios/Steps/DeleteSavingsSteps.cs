using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Threading;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteSavings")]
    public class DeleteSavingsSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _countSavings, _countMovements, _countIncomes, _sourceAccountId, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount, _costSaving;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly ISavingService _savingService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;

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
        
        [Given(@"I have at least one saving in the list")]
        public void GivenIHaveAtLeastOneSavingInTheList()
        {
            var savings = _ctx.WebDriver.FindElements(By.ClassName("trSaving"));
            if (savings.Count < 1)
            {
                throw new Exception("There is no saving to delete");
            }
            _firstRow = savings[0];
        }
        
        [When(@"I click on delete for the first saving")]
        public void WhenIClickOnDeleteForTheFirstSaving()
        {
            var costValue = _firstRow.FindElement(By.ClassName("tdAmount"));
            _costSaving = Convert.ToDecimal(costValue.Text.Substring(1));

            var targetAccountHid = _firstRow.FindElement(By.Id("item_TargetInternalAccountId"));
            _targetAccountId = Convert.ToInt32(targetAccountHid.GetAttribute("value"));
            _targetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);

            var deleteConfirmBtn = _firstRow.FindElement(By.ClassName("btn_delete"));
            deleteConfirmBtn.Click();
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            var deleteSavingPage = _ctx.WebDriver.FindElement(By.TagName("h5"));
            if (deleteSavingPage.Text != "Delete a saving")
            {
                throw new Exception("The confirmation of deletion should be there.");
            }
            var deleteBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_delete"));
            deleteBtn.Click();

            Thread.Sleep(2000);
        }

        [Then(@"the Saving has been removed")]
        public void ThenTheSavingHasBeenRemoved()
        {
            // Get Number Of Savings After
            var newCountSavings = _savingService.CountSavings();

            Assert.AreEqual(newCountSavings, _countSavings - 1);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + _costSaving);
        }
        
        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);

            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount - _costSaving);
        }
        
        [Then(@"an income has been removed")]
        public void ThenAnIncomeHasBeenRemoved()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
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
