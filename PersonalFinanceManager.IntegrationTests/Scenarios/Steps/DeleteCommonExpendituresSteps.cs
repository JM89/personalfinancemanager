using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteCommonExpenditures")]
    public class DeleteCommonExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countMovements;
        private decimal _sourceAccountAmount, _costExpenditure;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.ExpenseCreatePage.QuickCreateCommon();
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);

            SiteMap.ExpenseListPage.GoTo();

            _countExpenditures = DatabaseChecker.ExpenditureService.CountExpenditures();
            _countMovements = DatabaseChecker.HistoricMovementService.CountMovements();
        }

        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            _firstRow = SiteMap.ExpenseListPage.FindFirstRowAndCheckPaymentMethod("CB");

            _costExpenditure = SiteMap.ExpenseListPage.FindCost(_firstRow);
        }

        [When(@"I click on delete for this expenditure")]
        public void WhenIClickOnDeleteForThisExpenditure()
        {
            SiteMap.ExpenseListPage.ClickDeleteButton(_firstRow);
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            SiteMap.ExpenseListPage.CheckDeletionConfirmationModalTitle();
            SiteMap.ExpenseListPage.ClickConfirmDeletionButton();
        }

        [Then(@"the expenditure has been removed")]
        public void ThenTheExpenditureHasBeenRemoved()
        {
            var newCountExpenditures = DatabaseChecker.ExpenditureService.CountExpenditures();
            Assert.AreEqual(newCountExpenditures, _countExpenditures - 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + _costExpenditure);
        }
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementService.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
