using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateInternalTransferExpenditures")]
    public class CreateInternalTransferExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount;

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
        
        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            SiteMap.ExpenseListPage.ClickAddButton();
        }
        
        [When(@"I enter Description")]
        public void WhenIEnterDescription()
        {
            SiteMap.ExpenseCreatePage.EnterDescription("Internal Transfer");
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

        [When(@"I select the Internal Transfer payment type")]
        public void WhenISelectTheInternalTransferPaymentType()
        {
            SiteMap.ExpenseCreatePage.SelectPaymentMethod("Internal Transfer");
        }
        
        [When(@"I select the first target account")]
        public void WhenISelectTheFirstTargetAccount()
        {
            _targetAccountId = SiteMap.ExpenseCreatePage.SelectFirstTargetAccount();
            _targetAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_targetAccountId);
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

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_targetAccountId);
            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount + 100);
        }

        [Then(@"an income has been created")]
        public void ThenAnIncomeHasBeenCreated()
        {
            var newCountIncomes = DatabaseChecker.IncomeRepository.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes + 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }
    }
}
