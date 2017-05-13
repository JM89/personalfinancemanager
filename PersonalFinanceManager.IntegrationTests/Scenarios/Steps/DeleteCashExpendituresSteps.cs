using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteCashExpenditures")]
    public class DeleteCashExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countMovements, _atmWithdrawId;
        private decimal _sourceAccountAmount, _atmWithdrawAmount;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.ExpenseCreatePage.QuickCreateCash();
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);

            SiteMap.ExpenseListPage.GoTo();

            _countExpenditures = DatabaseChecker.ExpenditureRepository.CountExpenditures();
            _countMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
        }

        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            _firstRow = SiteMap.ExpenseListPage.FindFirstRowAndCheckPaymentMethod("Cash");

            _atmWithdrawId = SiteMap.ExpenseListPage.FindAtmWithdrawId(_firstRow);
            _atmWithdrawAmount = DatabaseChecker.AtmWithdrawRepository.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
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
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }

        [Then(@"the source account is unchanged")]
        public void ThenTheSourceAccountUnchanged()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount);
        }

        [Then(@"the target atm withdraw is updated")]
        public void ThenTheTargetAtmWithdrawIsUpdated()
        {
            var newTargetAtmWithdrawAmount = DatabaseChecker.AtmWithdrawRepository.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
            var expectedTargetAmount = _atmWithdrawAmount + 100;
            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }
    }
}
