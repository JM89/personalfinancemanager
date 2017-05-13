using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteInternalTransferExpenditures")]
    public class DeleteInternalTransferExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount, _costExpenditure;
        private IWebElement _firstRow;
        
        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.ExpenseCreatePage.QuickCreateInternalTransfer();
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);

            SiteMap.ExpenseListPage.GoTo();

            _countExpenditures = DatabaseChecker.ExpenditureRepository.CountExpenditures();
            _countIncomes = DatabaseChecker.IncomeRepository.CountIncomes();
            _countMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
        }

        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            _firstRow = SiteMap.ExpenseListPage.FindFirstRowAndCheckPaymentMethod("Internal Transfer");

            _targetAccountId = SiteMap.ExpenseListPage.FindTargetInternalAccountId(_firstRow);
            _targetAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_targetAccountId);
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
            var newCountExpenditures = DatabaseChecker.ExpenditureRepository.CountExpenditures();
            Assert.AreEqual(newCountExpenditures, _countExpenditures - 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + _costExpenditure);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_targetAccountId);
            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount - _costExpenditure);
        }

        [Then(@"an income has been removed")]
        public void ThenAnIncomeHasBeenRemoved()
        {
            var newCountIncomes = DatabaseChecker.IncomeRepository.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
