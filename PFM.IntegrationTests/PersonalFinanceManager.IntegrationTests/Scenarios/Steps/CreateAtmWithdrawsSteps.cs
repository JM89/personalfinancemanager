using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateAtmWithdraws")]
    public class CreateAtmWithdrawsSteps
    {
        private int _sourceAccountId, _countAtmWithdraws, _countMovements;
        private decimal _sourceAccountAmount;

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
        
        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            SiteMap.AtmWithdrawListPage.ClickAddButton();
        }
        
        [When(@"I enter Cost")]
        public void WhenIEnterCost()
        {
            SiteMap.AtmWithdrawCreatePage.EnterCost(100.00);
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.AtmWithdrawCreatePage.ClickSave();
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
        }
        
        [Then(@"an ATM withdraw has been created")]
        public void ThenAnAtmWithdrawHasBeenCreated()
        {
            var newCountAtmWithdraws = DatabaseChecker.CountAtmWithdraws();
            Assert.AreEqual(newCountAtmWithdraws, _countAtmWithdraws + 1);
        }
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
