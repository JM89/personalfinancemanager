﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.IntegrationTests.Scenarios.PreActions;
using PersonalFinanceManager.ServicesForTests;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Support.UI;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditInternalTransferExpenditures")]
    public class EditInternalTransferExpendituresSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _oldTargetAccountId, _newTargetAccountId, _expenditureId, _atmWithdrawId;
        private decimal _sourceAccountAmount, _oldTargetAccountAmount, _newTargetAccountAmount, _costExpenditure, _newCostExpenditure, _atmWithdrawAmount;
        private IWebElement _firstRow;

        private readonly IAtmWithdrawService _atmWithdrawService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;
        private readonly IExpenditureService _expenditureService;

        public EditInternalTransferExpendituresSteps()
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
            CreateInternalTransferExpenditures.Execute(_ctx);
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            _ctx.GotToUrl("/Expenditure/Index");

            // Get Source Account Amount Before Creating Expenditures
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

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

            var paymentMethod = _firstRow.FindElement(By.ClassName("tdPaymentMethod")).Text;
            if (paymentMethod != "Internal Transfer")
            {
                throw new Exception("There is no expenditure with payment method Internal Transfer to delete");
            }
        }
        
        [When(@"I click on edit for this expenditure")]
        public void WhenIClickOnEditForThisExpenditure()
        {
            var costValue = _firstRow.FindElement(By.ClassName("tdCost"));
            _costExpenditure = Convert.ToDecimal(costValue.Text.Substring(1));

            var editBtn = _firstRow.FindElement(By.ClassName("btn_edit"));
            editBtn.Click();

            var targetAccountHid = _ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId"));
            _oldTargetAccountId = Convert.ToInt32(targetAccountHid.GetAttribute("value"));
            _oldTargetAccountAmount = _bankAccountService.GetAccountAmount(_oldTargetAccountId);

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
        
        [When(@"I select another account")]
        public void WhenISelectAnotherAccount()
        {
            var accountDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            if (accountDdl.Options.Count < 3)
                throw new Exception("TargetInternalAccountId has no enough option. At least 2 expected.");

            var selectedValueOption = Convert.ToInt32(accountDdl.AllSelectedOptions[0].GetAttribute("value"));

            var savingAccounts = new List<Tuple<int, int, bool>>();
            for (var ind = 1; ind < accountDdl.Options.Count; ind++)
            {
                var value = Convert.ToInt32(accountDdl.Options[ind].GetAttribute("value"));
                savingAccounts.Add(new Tuple<int, int, bool>(ind, value, value == selectedValueOption));
            }

            var firstNotSelectedIndex = savingAccounts.First(x => !x.Item3);

            accountDdl.SelectByIndex(firstNotSelectedIndex.Item1);

            // Get New Target Account Amount Before Creating Savings
            _newTargetAccountId = firstNotSelectedIndex.Item2;
            _newTargetAccountAmount = _bankAccountService.GetAccountAmount(_newTargetAccountId);
        }
        
        [When(@"I change the payment method to Common Expenditures")]
        public void WhenIChangeThePaymentMethodToCommonExpenditures()
        {
            var paymentMethodDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("paymentMethodId")));
            paymentMethodDdl.SelectByText("CB");

            Thread.Sleep(2000);
        }
       
        [When(@"I change the payment method to Cash Expenditures")]
        public void WhenIChangeThePaymentMethodToCashExpenditures()
        {
            var paymentMethodDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("paymentMethodId")));
            paymentMethodDdl.SelectByText("Cash");

            Thread.Sleep(2000);
        }

        [When(@"I select an ATM withdraw")]
        public void WhenISelectAnAtmWithdraw()
        {
            var atmWithdrawDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("AtmWithdrawId")));
            if (atmWithdrawDdl.Options.Count < 2)
                throw new Exception("AtmWithdrawId has no enough option. At least 1 expected.");
            atmWithdrawDdl.SelectByIndex(1);

            _atmWithdrawId = Convert.ToInt32(atmWithdrawDdl.Options[1].GetAttribute("value"));
            _atmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_atmWithdrawId);
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

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            var expectedSourceAmount = _sourceAccountAmount + _costExpenditure - _newCostExpenditure;

            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the old source account is updated")]
        public void ThenTheOldSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            var expectedSourceAmount = _sourceAccountAmount + _costExpenditure;

            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_oldTargetAccountId);

            var expectedTargetAmount = _oldTargetAccountAmount - _costExpenditure + _newCostExpenditure;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the old target account is updated")]
        public void ThenTheOldTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_oldTargetAccountId);

            var expectedTargetAmount = _oldTargetAccountAmount - _costExpenditure;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the new target account is updated")]
        public void ThenTheNewTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_newTargetAccountId);

            var expectedTargetAmount = _newTargetAccountAmount + _newCostExpenditure;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the target atm withdraw is updated")]
        public void ThenTheTargetAtmWithdrawIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAtmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_atmWithdrawId);

            var expectedTargetAmount = _atmWithdrawAmount - _newCostExpenditure;

            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }

        [Then(@"an income has been updated")]
        public void ThenAnIncomeHasBeenUpdated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, _countIncomes);
        }

        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, _countMovements + 2);
        }

        [Then(@"an income has been removed")]
        public void ThenAnIncomeHasBeenRemoved()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
        }

        [AfterScenario]
        public void TestTearDown()
        {
            _ctx.StopTest();
        }
    }
}