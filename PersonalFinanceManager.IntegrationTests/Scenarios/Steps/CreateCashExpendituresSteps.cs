using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateCashExpenditures")]
    public class CreateCashExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countMovements, _atmWithdrawId;
        private decimal _sourceAccountAmount, _atmWithdrawAmount;

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

        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            SiteMap.ExpenseListPage.ClickAddButton();
        }

        [When(@"I enter Description")]
        public void WhenIEnterDescription()
        {
            SiteMap.ExpenseCreatePage.EnterDescription("Cash");
        }

        [When(@"I enter a Cost")]
        public void WhenIEnterACost()
        {
            SiteMap.ExpenseCreatePage.EnterCost(100);
        }

        [When(@"I select the first expenditure type")]
        public void WhenISelectTheFirstExpenditureType()
        {
            SiteMap.ExpenseCreatePage.SelectFirstExpenseType();
        }

        [When(@"I select the Cash payment type")]
        public void WhenISelectTheCashPaymentType()
        {
            SiteMap.ExpenseCreatePage.SelectPaymentMethod("Cash");
        }

        [When(@"I select an ATM withdraw")]
        public void WhenISelectAnAtmWithdraw()
        {
            _atmWithdrawId = SiteMap.ExpenseCreatePage.SelectFirstAtmWithdraw();
            _atmWithdrawAmount = DatabaseChecker.AtmWithdrawRepository.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.ExpenseCreatePage.ClickSave();
        }

        [Then(@"the Expenditure Has Been Created")]
        public void ThenTheExpenditureHasBeenCreated()
        {
            var newCountExpenditures = DatabaseChecker.ExpenditureRepository.CountExpenditures();
            Assert.AreEqual(newCountExpenditures, _countExpenditures + 1);
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
            var expectedTargetAmount = _atmWithdrawAmount - 100;
            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
