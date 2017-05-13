using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditAtmWithdraws")]
    public class EditAtmWithdrawsSteps
    {
        private int _sourceAccountId, _atmWithdrawId, _countAtmWithdraws, _countMovements;
        private decimal _sourceAccountAmount, _costAtmWithdraw, _newCostAtmWithdraw;
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
            _sourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);

            SiteMap.AtmWithdrawListPage.GoTo();

            _countAtmWithdraws = DatabaseChecker.AtmWithdrawService.CountAtmWithdraws();
            _countMovements = DatabaseChecker.HistoricMovementService.CountMovements();
        }

        [Given(@"I have at least one ATM withdraw in the list")]
        public void GivenIHaveAtLeastOneAtmWithdrawInTheList()
        {
            _firstRow = SiteMap.AtmWithdrawListPage.FindFirstRow();
        }
        
        [When(@"I click on edit for the first ATM withdraw")]
        public void WhenIClickOnEditForTheFirstAtmWithdraw()
        {
            _costAtmWithdraw = SiteMap.AtmWithdrawListPage.FindCost(_firstRow);

            SiteMap.AtmWithdrawListPage.ClickEditButton(_firstRow);

            _atmWithdrawId = SiteMap.AtmWithdrawEditPage.FindAtmWithdrawId();
        }
        
        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            SiteMap.AtmWithdrawEditPage.EnterCost(_costAtmWithdraw + 100);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.AtmWithdrawEditPage.ClickSave();
        }
        
        [Then(@"the ATM withdraw has been updated")]
        public void ThenTheAtmWithdrawHasBeenUpdated()
        {
            var newCountAtmWithdraws = DatabaseChecker.AtmWithdrawService.CountAtmWithdraws();
            Assert.AreEqual(newCountAtmWithdraws, _countAtmWithdraws);

            _newCostAtmWithdraw = DatabaseChecker.AtmWithdrawService.GetAtmWithdrawInitialAmount(_atmWithdrawId);
            Assert.AreEqual(_costAtmWithdraw + 100, _newCostAtmWithdraw);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount + _costAtmWithdraw - _newCostAtmWithdraw;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementService.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }
    }
}
