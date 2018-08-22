using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteIncomes")]
    public class DeleteIncomesSteps
    {
        private int _countMovements, _countIncomes, _sourceAccountId;
        private decimal _sourceAccountAmount;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.IncomeCreatePage.QuickCreate();
        }

        [Given(@"I have accessed the Income List page")]
        public void GivenIHaveAccessedTheIncomeListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);

            SiteMap.IncomeListPage.GoTo();

            _countIncomes = DatabaseChecker.CountIncomes();
            _countMovements = DatabaseChecker.CountMovements();
        }

        [Given(@"I have at least one income in the list")]
        public void GivenIHaveAtLeastOneIncomeInTheList()
        {
            _firstRow = SiteMap.IncomeListPage.FindFirstRow();
        }

        [When(@"I click on delete for the first income")]
        public void WhenIClickOnDeleteForTheFirstIncome()
        {
            SiteMap.IncomeListPage.ClickDeleteButton(_firstRow);
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            SiteMap.IncomeListPage.CheckDeletionConfirmationModalTitle();
            SiteMap.IncomeListPage.ClickConfirmDeletionButton();
        }

        [Then(@"the income has been removed")]
        public void ThenTheIncomeHasBeenRemoved()
        {
            var newCountIncomes = DatabaseChecker.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
        }
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
