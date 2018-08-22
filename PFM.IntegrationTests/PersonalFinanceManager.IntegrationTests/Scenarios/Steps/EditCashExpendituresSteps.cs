using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditCashExpenditures")]
    public class EditCashExpendituresSteps
    {
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _expenditureId, _oldAtmWithdrawId, _newAtmWithdrawId, _targetAccountId;
        private decimal _sourceAccountAmount, _costExpenditure, _newCostExpenditure, _oldAtmWithdrawAmount, _newAtmWithdrawAmount, _targetAccountAmount;
        private IWebElement _firstRow;

        [BeforeScenario]
        public void PrepareForTest()
        {
            SiteMap.ExpenseCreatePage.QuickCreateCash();
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            SiteMap.AccountManagementDashboardPage.GoTo();
            _sourceAccountId = SiteMap.AccountManagementDashboardPage.SelectAccount();
            _sourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);

            SiteMap.ExpenseListPage.GoTo();

            _countExpenditures = DatabaseChecker.CountExpenditures();
            _countIncomes = DatabaseChecker.CountIncomes();
            _countMovements = DatabaseChecker.CountMovements();
        }

        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            _firstRow = SiteMap.ExpenseListPage.FindFirstRowAndCheckPaymentMethod("Cash");
        }
        
        [When(@"I click on edit for this expenditure")]
        public void WhenIClickOnEditForThisExpenditure()
        {
            _costExpenditure = SiteMap.ExpenseListPage.FindCost(_firstRow);

            SiteMap.ExpenseListPage.ClickEditButton(_firstRow);

            _oldAtmWithdrawId = SiteMap.ExpenseEditPage.FindAtmWithdrawId();
            _oldAtmWithdrawAmount = DatabaseChecker.GetAtmWithdrawCurrentAmount(_oldAtmWithdrawId);

            _expenditureId = SiteMap.ExpenseEditPage.FindExpenseId();
        }

        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            SiteMap.ExpenseEditPage.EnterCost(_costExpenditure + 100);
        }

        [When(@"I select another ATM withdraw")]
        public void WhenISelectAnotherAtmWithdraw()
        {
            _newAtmWithdrawId = SiteMap.ExpenseEditPage.SelectAnotherAtmWithdraw();
            _newAtmWithdrawAmount = DatabaseChecker.GetAtmWithdrawCurrentAmount(_newAtmWithdrawId);
        }

        [When(@"I change the payment method to Common Expenditures")]
        public void WhenIChangeThePaymentMethodToCommonExpenditures()
        {
            SiteMap.ExpenseEditPage.SelectPaymentMethod("CB");
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
            _targetAccountAmount = DatabaseChecker.GetBankAccountAmount(_targetAccountId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.ExpenseEditPage.ClickSave();
        }

        [Then(@"the expenditure has been updated")]
        public void ThenTheExpenditureHasBeenUpdated()
        {
            var newCountExpenditures = DatabaseChecker.CountExpenditures();
            Assert.AreEqual(newCountExpenditures, _countExpenditures);

            _newCostExpenditure = DatabaseChecker.GetExpenditureCost(_expenditureId);
            Assert.AreEqual(_costExpenditure + 100, _newCostExpenditure);
        }

        [Then(@"the source account is unchanged")]
        public void ThenTheSourceAccountUnchanged()
        {
            var newSourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);
            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            var newSourceAccountAmount = DatabaseChecker.GetBankAccountAmount(_sourceAccountId);
            var expectedSourceAmount = _sourceAccountAmount - _newCostExpenditure;
            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the target atm withdraw is updated")]
        public void ThenTheTargetAtmWithdrawIsUpdated()
        {
            var newTargetAtmWithdrawAmount = DatabaseChecker.GetAtmWithdrawCurrentAmount(_oldAtmWithdrawId);
            var expectedTargetAmount = _oldAtmWithdrawAmount + _costExpenditure - _newCostExpenditure; 
            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }

        [Then(@"the old atm withdraw is updated")]
        public void ThenTheOldTargetAccountIsUpdated()
        {
            var newAtmWithdrawAmount = DatabaseChecker.GetAtmWithdrawCurrentAmount(_oldAtmWithdrawId);
            var expectedAtmWithdrawAmount = _oldAtmWithdrawAmount + _costExpenditure;
            Assert.AreEqual(expectedAtmWithdrawAmount, newAtmWithdrawAmount);
        }

        [Then(@"the new atm withdraw is updated")]
        public void ThenTheNewTargetAccountIsUpdated()
        {
            var newAtmWithdrawAmount = DatabaseChecker.GetAtmWithdrawCurrentAmount(_newAtmWithdrawId);
            var expectedAtmWithdrawAmount = _newAtmWithdrawAmount - _newCostExpenditure;
            Assert.AreEqual(expectedAtmWithdrawAmount, newAtmWithdrawAmount);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            var newTargetAccountAmount = DatabaseChecker.GetBankAccountAmount(_targetAccountId);
            var expectedTargetAmount = _targetAccountAmount + _newCostExpenditure;
            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"an income has been updated")]
        public void ThenAnIncomeHasBeenUpdated()
        {
            var newCountIncomes = DatabaseChecker.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes + 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            var newCountMovements = DatabaseChecker.CountMovements();
            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }
    }
}
