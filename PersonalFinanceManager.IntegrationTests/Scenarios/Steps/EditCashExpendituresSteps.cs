using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.IntegrationTests.Scenarios.PreActions;
using PersonalFinanceManager.ServicesForTests;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditCashExpenditures")]
    public class EditCashExpendituresSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _expenditureId, _oldAtmWithdrawId, _newAtmWithdrawId, _targetAccountId;
        private decimal _sourceAccountAmount, _costExpenditure, _newCostExpenditure, _oldAtmWithdrawAmount, _newAtmWithdrawAmount, _targetAccountAmount;
        private IWebElement _firstRow;

        private readonly IAtmWithdrawService _atmWithdrawService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;
        private readonly IExpenditureService _expenditureService;

        public EditCashExpendituresSteps()
        {
            _atmWithdrawService = new AtmWithdrawService();
            _bankAccountService = new BankAccountService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
            _expenditureService = new ExpenditureService();
        }

        [BeforeScenario]
        public void PrepareForTest()
        {
            CreateCashExpenditures.Execute(_ctx);
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            // Get Source Account Amount Before Creating Expenditures
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            _ctx.GotToUrl("/Expenditure/Index");

            // Get Number Of Savings Before Creating Expenditures
            _countExpenditures = _expenditureService.CountExpenditures();

            // Get Number Of Incomes Before Creating Expenditures
            _countIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Expenditures
            _countMovements = _historicMovementService.CountMovements();
        }

        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            var expenditures = _ctx.WebDriver.FindElements(By.ClassName("trExpenditure"));
            if (expenditures.Count < 1)
            {
                throw new Exception("There is no expenditure to delete");
            }

            _firstRow = expenditures[0];

            var paymentMethod = _firstRow.FindElement(By.ClassName("tdPaymentMethod")).FindElement(By.Id("item_PaymentMethodName")).GetAttribute("value");
            if (paymentMethod != "Cash")
            {
                throw new Exception("There is no expenditure with payment method Cash to delete");
            }
        }
        
        [When(@"I click on edit for this expenditure")]
        public void WhenIClickOnEditForThisExpenditure()
        {
            var costValue = _firstRow.FindElement(By.ClassName("tdCost"));
            _costExpenditure = Convert.ToDecimal(costValue.Text.Substring(1));

            var editBtn = _firstRow.FindElement(By.ClassName("btn_edit"));
            editBtn.Click();
            
            var atmWithdrawHid = _ctx.WebDriver.FindElement(By.Id("AtmWithdrawId"));
            _oldAtmWithdrawId = Convert.ToInt32(atmWithdrawHid.GetAttribute("value"));
            _oldAtmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_oldAtmWithdrawId);

            var expenditureIdHid = _ctx.WebDriver.FindElement(By.Id("Id"));
            _expenditureId = Convert.ToInt32(expenditureIdHid.GetAttribute("value"));
        }

        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            var costTxt = _ctx.WebDriver.FindElement(By.Id("Cost"));
            costTxt.Clear();
            costTxt.SendKeys((_costExpenditure + 100).ToString());
        }

        [When(@"I select another ATM withdraw")]
        public void WhenISelectAnotherAtmWithdraw()
        {
            var atwWithdrawDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("AtmWithdrawId")));
            if (atwWithdrawDdl.Options.Count < 3)
                throw new Exception("AtmWithdrawId has no enough option. At least 2 expected.");

            var selectedValueOption = Convert.ToInt32(atwWithdrawDdl.AllSelectedOptions[0].GetAttribute("value"));

            var atmWithdraws = new List<Tuple<int, int, bool>>();
            for (var ind = 1; ind < atwWithdrawDdl.Options.Count; ind++)
            {
                var value = Convert.ToInt32(atwWithdrawDdl.Options[ind].GetAttribute("value"));
                atmWithdraws.Add(new Tuple<int, int, bool>(ind, value, value == selectedValueOption));
            }

            var firstNotSelectedIndex = atmWithdraws.First(x => !x.Item3);

            atwWithdrawDdl.SelectByIndex(firstNotSelectedIndex.Item1);

            // Get New Target ATM Amount Before Creating Expenditures
            _newAtmWithdrawId = firstNotSelectedIndex.Item2;
            _newAtmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_newAtmWithdrawId);
        }

        [When(@"I change the payment method to Common Expenditures")]
        public void WhenIChangeThePaymentMethodToCommonExpenditures()
        {
            var paymentMethodDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("paymentMethodId")));
            paymentMethodDdl.SelectByText("CB");

            Thread.Sleep(2000);
        }

        [When(@"I change the payment method to Internal Transfer Expenditures")]
        public void WhenIChangeThePaymentMethodToInternalTransferExpenditures()
        {
            var paymentMethodDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("paymentMethodId")));
            paymentMethodDdl.SelectByText("Internal Transfer");

            Thread.Sleep(2000);
        }

        [When(@"I select a target account")]
        public void WhenISelectATargetAccount()
        {
            var accountDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            if (accountDdl.Options.Count < 2)
                throw new Exception("TargetInternalAccountId has no enough option. At least 1 expected.");
            accountDdl.SelectByIndex(1);

            _targetAccountId = Convert.ToInt32(accountDdl.Options[1].GetAttribute("value"));
            _targetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }

        [Then(@"the expenditure has been updated")]
        public void ThenTheExpenditureHasBeenUpdated()
        {
            // Get Number Of Expenditures After
            var newCountExpenditures = _expenditureService.CountExpenditures();
            Assert.AreEqual(newCountExpenditures, _countExpenditures);

            _newCostExpenditure = _expenditureService.GetExpenditureCost(_expenditureId);
            Assert.AreEqual(_costExpenditure + 100, _newCostExpenditure);
        }

        [Then(@"the source account is unchanged")]
        public void ThenTheSourceAccountUnchanged()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            var expectedSourceAmount = _sourceAccountAmount - _newCostExpenditure;

            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the target atm withdraw is updated")]
        public void ThenTheTargetAtmWithdrawIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAtmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_oldAtmWithdrawId);

            var expectedTargetAmount = _oldAtmWithdrawAmount + _costExpenditure - _newCostExpenditure; 

            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }

        [Then(@"the old atm withdraw is updated")]
        public void ThenTheOldTargetAccountIsUpdated()
        {
            var newAtmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_oldAtmWithdrawId);

            var expectedAtmWithdrawAmount = _oldAtmWithdrawAmount + _costExpenditure;

            Assert.AreEqual(expectedAtmWithdrawAmount, newAtmWithdrawAmount);
        }

        [Then(@"the new atm withdraw is updated")]
        public void ThenTheNewTargetAccountIsUpdated()
        {
            var newAtmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_newAtmWithdrawId);

            var expectedAtmWithdrawAmount = _newAtmWithdrawAmount - _newCostExpenditure;

            Assert.AreEqual(expectedAtmWithdrawAmount, newAtmWithdrawAmount);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);

            var expectedTargetAmount = _targetAccountAmount + _newCostExpenditure;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"an income has been updated")]
        public void ThenAnIncomeHasBeenUpdated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, _countIncomes + 1);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }

        [AfterScenario]
        public void TestTearDown()
        {
            _ctx.StopTest();
        }
    }
}
