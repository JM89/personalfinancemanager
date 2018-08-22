using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditSavings")]
    public class EditSavingsSteps
    {
        private int _countSavings, _countMovements, _countIncomes, _sourceAccountId, _oldTargetAccountId, _newTargetAccountId, _savingId;
        private decimal _sourceAccountAmount, _oldTargetAccountAmount, _newTargetAccountAmount, _costSaving, _newCostSaving;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.SavingCreatePage.QuickCreate();
        }

        [Given(@"I have accessed the Saving List page")]
        public void GivenIHaveAccessedTheSavingListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);

            SiteMap.SavingListPage.GoTo();

            _countSavings = DatabaseChecker.CountSavings();
            _countIncomes = DatabaseChecker.CountIncomes();
            _countMovements = DatabaseChecker.CountMovements();
        }
        
        [Given(@"I have at least one saving in the list")]
        public void GivenIHaveAtLeastOneSavingInTheList()
        {
            _firstRow = SiteMap.SavingListPage.FindFirstRow();

            _costSaving = SiteMap.SavingListPage.FindCost(_firstRow);
            _oldTargetAccountId = SiteMap.SavingListPage.FindTargetInternalAccountId(_firstRow);
            _oldTargetAccountAmount = DatabaseChecker.GetBankAccountAmount(_oldTargetAccountId);
        }
        
        [When(@"I click on edit for the first saving")]
        public void WhenIClickOnEditForTheFirstSaving()
        {
            SiteMap.SavingListPage.ClickEditButton(_firstRow);
            _savingId = SiteMap.SavingEditPage.FindSavingId();
        }
        
        [When(@"I edit the Amount")]
        public void WhenIEditTheAmount()
        {
            SiteMap.SavingEditPage.EnterAmount(_costSaving + 100);
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.SavingEditPage.ClickSave();
        }
        
        [When(@"I select another account")]
        public void WhenISelectAnotherAccount()
        {
            _newTargetAccountId = SiteMap.SavingEditPage.SelectAnotherSavingAccount();
            _newTargetAccountAmount = DatabaseChecker.GetBankAccountAmount(_newTargetAccountId);
        }
        
        [Then(@"the Saving has been updated")]
        public void ThenTheSavingHasBeenUpdated()
        {
            var newCountSavings = DatabaseChecker.CountSavings();
            Assert.AreEqual(newCountSavings, _countSavings);

            _newCostSaving = DatabaseChecker.GetSavingCost(_savingId);
            Assert.AreEqual(_costSaving + 100, _newCostSaving);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount + _costSaving - _newCostSaving;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }
        
        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.GetBankAccountAmount(_oldTargetAccountId);
            var expectedTargetAmount = _oldTargetAccountAmount - _costSaving + _newCostSaving;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the old target account is updated")]
        public void ThenTheOldTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.GetBankAccountAmount(_oldTargetAccountId);
            var expectedTargetAmount = _oldTargetAccountAmount - _costSaving;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the new target account is updated")]
        public void ThenTheNewTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.GetBankAccountAmount(_newTargetAccountId);
            var expectedTargetAmount = _newTargetAccountAmount + _newCostSaving;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"an income has been updated")]
        public void ThenAnIncomeHasBeenUpdated()
        {
            var newCountIncomes = DatabaseChecker.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes);
        }
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }
    }
}
