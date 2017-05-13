using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateSavings")]
    public class CreateSavingsSteps
    {
        private int _countSavings, _countMovements, _countIncomes, _sourceAccountId, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount;

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
        
        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            SiteMap.SavingListPage.ClickAddButton();
        }
        
        [When(@"I enter Amount")]
        public void WhenIEnterAmount()
        {
            SiteMap.SavingCreatePage.EnterAmount(100);
        }

        [When(@"I select the first Saving Account")]
        public void WhenISelectTheFirstSavingAccount()
        {
            _targetAccountId = SiteMap.SavingCreatePage.SelectFirstSavingAccount();
            _targetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_targetAccountId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.SavingCreatePage.ClickSave();
        }
        
        [Then(@"the Saving Has Been Created")]
        public void ThenTheSavingHasBeenCreated()
        {
            var newCountSavings = DatabaseChecker.SavingService.CountSavings();
            Assert.AreEqual(newCountSavings, _countSavings + 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_targetAccountId);
            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount + 100);
        }

        [Then(@"an income has been created")]
        public void ThenAnIncomeHasBeenCreated()
        {
            var newCountIncomes = DatabaseChecker.IncomeService.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes + 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementService.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
