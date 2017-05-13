using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Threading;

using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteSavings")]
    public class DeleteSavingsSteps
    {
        private int _countSavings, _countMovements, _countIncomes, _sourceAccountId, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount, _costSaving;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.SavingCreatePage.QuickCreate();
        }

        [Given(@"I have accessed the Saving List page")]
        public void GivenIHaveAccessedTheSavingListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);

            SiteMap.SavingListPage.GoTo();

            _countSavings = DatabaseChecker.SavingService.CountSavings();
            _countIncomes = DatabaseChecker.IncomeService.CountIncomes();
            _countMovements = DatabaseChecker.HistoricMovementService.CountMovements();
        }
        
        [Given(@"I have at least one saving in the list")]
        public void GivenIHaveAtLeastOneSavingInTheList()
        {
            _firstRow = SiteMap.SavingListPage.FindFirstRow();

            _costSaving = SiteMap.SavingListPage.FindCost(_firstRow);
            _targetAccountId = SiteMap.SavingListPage.FindTargetInternalAccountId(_firstRow);
            _targetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_targetAccountId);
        }

        [When(@"I click on delete for the first saving")]
        public void WhenIClickOnDeleteForTheFirstSaving()
        {
            SiteMap.SavingListPage.ClickDeleteButton(_firstRow);
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            SiteMap.SavingListPage.CheckDeletionConfirmationModalTitle();
            SiteMap.SavingListPage.ClickConfirmDeletionButton();
        }

        [Then(@"the Saving has been removed")]
        public void ThenTheSavingHasBeenRemoved()
        {
            var newCountSavings = DatabaseChecker.SavingService.CountSavings();
            Assert.AreEqual(newCountSavings, _countSavings - 1);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + _costSaving);
        }
        
        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_targetAccountId);
            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount - _costSaving);
        }
        
        [Then(@"an income has been removed")]
        public void ThenAnIncomeHasBeenRemoved()
        {
            var newCountIncomes = DatabaseChecker.IncomeService.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
        }
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementService.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
