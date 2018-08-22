using PersonalFinanceManager.IntegrationTests.Infrastructure;
using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding]
    public class DeleteAtmWithdrawsSteps
    {
        private int _sourceAccountId, _countAtmWithdraws, _countMovements;
        private decimal _sourceAccountAmount;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.AtmWithdrawCreatePage.QuickCreate();
        }

        [Given(@"I have accessed the ATM Withdraw List page")]
        public void GivenIHaveAccessedTheAtmWithdrawListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);

            SiteMap.AtmWithdrawListPage.GoTo();

            _countAtmWithdraws = DatabaseChecker.CountAtmWithdraws();
            _countMovements = DatabaseChecker.CountMovements();
        }

        [Given(@"I have at least one ATM Withdraw in the list")]
        public void GivenIHaveAtLeastOneAtmWithdrawInTheList()
        {
            _firstRow = SiteMap.AtmWithdrawListPage.FindFirstRow();
        }
        
        [When(@"I click on delete for the first ATM Withdraw")]
        public void WhenIClickOnDeleteForTheFirstAtmWithdraw()
        {
            SiteMap.AtmWithdrawListPage.ClickDeleteButton(_firstRow);
        }
        
        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            SiteMap.AtmWithdrawListPage.CheckDeletionConfirmationModalTitle();
            SiteMap.AtmWithdrawListPage.ClickConfirmDeletionButton();
        }
        
        [Then(@"the ATM Withdraw has been removed")]
        public void ThenTheAtmWithdrawHasBeenRemoved()
        {
            var newCountAtmWithdraws = DatabaseChecker.CountAtmWithdraws();
            Assert.AreEqual(newCountAtmWithdraws, _countAtmWithdraws - 1);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + 100);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
