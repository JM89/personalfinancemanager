using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditCommonExpenditures")]
    public class EditCommonExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _targetAccountId, _expenditureId, _atmWithdrawId;
        private decimal _sourceAccountAmount, _targetAccountAmount, _costExpenditure, _newCostExpenditure, _atmWithdrawAmount;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.ExpenseCreatePage.QuickCreateCommon();
        }

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

        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            _firstRow = SiteMap.ExpenseListPage.FindFirstRowAndCheckPaymentMethod("CB");
        }

        [When(@"I click on edit for this expenditure")]
        public void WhenIClickOnEditForThisExpenditure()
        {
            _costExpenditure = SiteMap.ExpenseListPage.FindCost(_firstRow);

            SiteMap.ExpenseListPage.ClickEditButton(_firstRow);

            _expenditureId = SiteMap.ExpenseEditPage.FindExpenseId();
        }

        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            SiteMap.ExpenseEditPage.EnterCost(_costExpenditure + 100);
        }
        
        [When(@"I select an ATM withdraw")]
        public void WhenISelectAnAtmWithdraw()
        {
            _atmWithdrawId = SiteMap.ExpenseEditPage.SelectFirstAtmWithdraw();
            _atmWithdrawAmount = DatabaseChecker.AtmWithdrawRepository.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
        }

        [When(@"I change the payment method to Cash Expenditures")]
        public void WhenIChangeThePaymentMethodToCashExpenditures()
        {
            SiteMap.ExpenseEditPage.SelectPaymentMethod("Cash");
        }

        [When(@"I change the payment method to Internal Transfer Expenditures")]
        public void WhenIChangeThePaymentMethodToInternalTransferExpenditures()
        {
            SiteMap.ExpenseEditPage.SelectPaymentMethod("Internal Transfer");
        }

        [When(@"I select a target account")]
        public void WhenISelectATargetAccount()
        {
            _targetAccountId = SiteMap.ExpenseEditPage.SelectFirstTargetAccount();
            _targetAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_targetAccountId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.ExpenseEditPage.ClickSave();
        }

        [Then(@"the expenditure has been updated")]
        public void ThenTheExpenditureHasBeenUpdated()
        {
            var newCountExpenditures = DatabaseChecker.ExpenditureRepository.CountExpenditures();
            Assert.AreEqual(newCountExpenditures, _countExpenditures);

            _newCostExpenditure = DatabaseChecker.ExpenditureRepository.GetExpenditureCost(_expenditureId);
            Assert.AreEqual(_costExpenditure + 100, _newCostExpenditure);
        }

        [Then(@"the old source account is updated")]
        public void ThenTheOldSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount + _costExpenditure;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount + _costExpenditure - _newCostExpenditure;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the target atm withdraw is updated")]
        public void ThenTheTargetAtmWithdrawIsUpdated()
        {
            var newTargetAtmWithdrawAmount = DatabaseChecker.AtmWithdrawRepository.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
            var expectedTargetAmount = _atmWithdrawAmount - _newCostExpenditure;
            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }
        
        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.BankAccountRepository.GetAccountAmount(_targetAccountId);
            var expectedTargetAmount = _targetAccountAmount + _newCostExpenditure;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"an income has been updated")]
        public void ThenAnIncomeHasBeenUpdated()
        {
            var newCountIncomes = DatabaseChecker.IncomeRepository.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes + 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.HistoricMovementRepository.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }
    }
}
