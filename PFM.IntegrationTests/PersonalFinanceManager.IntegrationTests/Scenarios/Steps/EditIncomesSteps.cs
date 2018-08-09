using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditIncomes")]
    public class EditIncomesSteps
    {
        private int _sourceAccountId, _incomeId, _countIncomes, _countMovements;
        private decimal _sourceAccountAmount, _costIncome, _newCostIncome;
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
            _sourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);

            SiteMap.IncomeListPage.GoTo();

            _countIncomes = DatabaseChecker.IncomeRepository.CountIncomes();
            _countMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
        }

        [Given(@"I have at least one income in the list")]
        public void GivenIHaveAtLeastOneIncomeInTheList()
        {
            _firstRow = SiteMap.IncomeListPage.FindFirstRow();
        }
        
        [When(@"I click on edit for the first income")]
        public void WhenIClickOnEditForTheFirstIncome()
        {
            _costIncome = SiteMap.IncomeListPage.FindCost(_firstRow);

            SiteMap.IncomeListPage.ClickEditButton(_firstRow);

            _incomeId = SiteMap.IncomeEditPage.FindIncomeId();
        }
        
        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            SiteMap.IncomeEditPage.EnterCost(_costIncome + 100);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.IncomeEditPage.ClickSave();
        }

        [Then(@"the Income has been updated")]
        public void ThenTheIncomeHasBeenUpdated()
        {
            var newCountIncomes = DatabaseChecker.IncomeRepository.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes);

            _newCostIncome = DatabaseChecker.IncomeRepository.GetIncomeCost(_incomeId);
            Assert.AreEqual(_costIncome + 100, _newCostIncome);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount - _costIncome + _newCostIncome;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }
    }
}
