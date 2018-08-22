using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateCommonExpenditures")]
    public class CreateCommonExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countMovements;
        private decimal _sourceAccountAmount;

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();

            _sourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);

            SiteMap.ExpenseListPage.GoTo();

            _countExpenditures = DatabaseChecker.CountExpenditures();
            _countMovements = DatabaseChecker.CountMovements();
        }

        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            SiteMap.ExpenseListPage.ClickAddButton();
        }

        [When(@"I enter Description")]
        public void WhenIEnterDescription()
        {
            SiteMap.ExpenseCreatePage.EnterDescription("Common Expenditure");
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

        [When(@"I select the CB payment type")]
        public void WhenISelectTheCbPaymentType()
        {
            SiteMap.ExpenseCreatePage.SelectPaymentMethod("CB");
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.ExpenseCreatePage.ClickSave();
        }

        [Then(@"the Expenditure Has Been Created")]
        public void ThenTheExpenditureHasBeenCreated()
        {
            var newCountExpenditures = DatabaseChecker.CountExpenditures();

            Assert.AreEqual(newCountExpenditures, _countExpenditures + 1);
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
            // Get Number Of Movements After
            var newCountMovements = DatabaseChecker.CountMovements();

            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
