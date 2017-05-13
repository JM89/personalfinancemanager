using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditInternalTransferExpenditures")]
    public class EditInternalTransferExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _oldTargetAccountId, _newTargetAccountId, _expenditureId, _atmWithdrawId;
        private decimal _sourceAccountAmount, _oldTargetAccountAmount, _newTargetAccountAmount, _costExpenditure, _newCostExpenditure, _atmWithdrawAmount;
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
            _sourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);

            SiteMap.ExpenseListPage.GoTo();

            _countExpenditures = DatabaseChecker.ExpenditureService.CountExpenditures();
            _countIncomes = DatabaseChecker.IncomeService.CountIncomes();
            _countMovements = DatabaseChecker.HistoricMovementService.CountMovements();
        }
        
        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            _firstRow = SiteMap.ExpenseListPage.FindFirstRowAndCheckPaymentMethod("Internal Transfer");
        }
        
        [When(@"I click on edit for this expenditure")]
        public void WhenIClickOnEditForThisExpenditure()
        {
            _costExpenditure = SiteMap.ExpenseListPage.FindCost(_firstRow);

            SiteMap.ExpenseListPage.ClickEditButton(_firstRow);

            _oldTargetAccountId = SiteMap.ExpenseEditPage.FindTargetAccountId();
            _oldTargetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_oldTargetAccountId);

            _expenditureId = SiteMap.ExpenseEditPage.FindExpenseId();
        }
        
        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            SiteMap.ExpenseEditPage.EnterCost(_costExpenditure + 100);
        }
        
        [When(@"I select another account")]
        public void WhenISelectAnotherAccount()
        {
            _newTargetAccountId = SiteMap.ExpenseEditPage.SelectAnotherTargetAccount();
            _newTargetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_newTargetAccountId);
        }
        
        [When(@"I change the payment method to Common Expenditures")]
        public void WhenIChangeThePaymentMethodToCommonExpenditures()
        {
            SiteMap.ExpenseEditPage.SelectPaymentMethod("CB");
        }
       
        [When(@"I change the payment method to Cash Expenditures")]
        public void WhenIChangeThePaymentMethodToCashExpenditures()
        {
            SiteMap.ExpenseEditPage.SelectPaymentMethod("Cash");
        }

        [When(@"I select an ATM withdraw")]
        public void WhenISelectAnAtmWithdraw()
        {
            _atmWithdrawId = SiteMap.ExpenseEditPage.SelectFirstAtmWithdraw();
            _atmWithdrawAmount = DatabaseChecker.AtmWithdrawService.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.ExpenseEditPage.ClickSave();
        }
        
        [Then(@"the expenditure has been updated")]
        public void ThenTheExpenditureHasBeenUpdated()
        {
            var newCountExpenditures = DatabaseChecker.ExpenditureService.CountExpenditures();
            Assert.AreEqual(newCountExpenditures, _countExpenditures);

            _newCostExpenditure = DatabaseChecker.ExpenditureService.GetExpenditureCost(_expenditureId);
            Assert.AreEqual(_costExpenditure + 100, _newCostExpenditure);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount + _costExpenditure - _newCostExpenditure;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the old source account is updated")]
        public void ThenTheOldSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount + _costExpenditure;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_oldTargetAccountId);
            var expectedTargetAmount = _oldTargetAccountAmount - _costExpenditure + _newCostExpenditure;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the old target account is updated")]
        public void ThenTheOldTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_oldTargetAccountId);
            var expectedTargetAmount = _oldTargetAccountAmount - _costExpenditure;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the new target account is updated")]
        public void ThenTheNewTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountService.GetAccountAmount(_newTargetAccountId);
            var expectedTargetAmount = _newTargetAccountAmount + _newCostExpenditure;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the target atm withdraw is updated")]
        public void ThenTheTargetAtmWithdrawIsUpdated()
        {
            var newTargetAtmWithdrawAmount = DatabaseChecker.AtmWithdrawService.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
            var expectedTargetAmount = _atmWithdrawAmount - _newCostExpenditure;
            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }

        [Then(@"an income has been updated")]
        public void ThenAnIncomeHasBeenUpdated()
        {
            var newCountIncomes = DatabaseChecker.IncomeService.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementService.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }

        [Then(@"an income has been removed")]
        public void ThenAnIncomeHasBeenRemoved()
        {
            var newCountIncomes = DatabaseChecker.IncomeService.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
        }
    }
}
