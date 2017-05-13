using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateIncomes")]
    public class CreateIncomesSteps
    {
        private int _sourceAccountId, _countIncomes, _countMovements;
        private decimal _sourceAccountAmount;
        
        [Given(@"I have accessed the Income List page")]
        public void GivenIHaveAccessedTheIncomeListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);

            SiteMap.IncomeListPage.GoTo();

            _countIncomes = DatabaseChecker.IncomeService.CountIncomes();
            _countMovements = DatabaseChecker.HistoricMovementService.CountMovements();
        }

        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            SiteMap.IncomeListPage.ClickAddButton();
        }
        
        [When(@"I enter a description")]
        public void WhenIEnterADescription()
        {
            SiteMap.IncomeCreatePage.EnterDescription("Income Description");
        }
        
        [When(@"I enter Cost")]
        public void WhenIEnterCost()
        {
            SiteMap.IncomeCreatePage.EnterCost(100);
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.IncomeCreatePage.ClickSave();
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + 100);
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
