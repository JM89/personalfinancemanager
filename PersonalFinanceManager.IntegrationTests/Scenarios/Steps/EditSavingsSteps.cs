using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditSavings")]
    public class EditSavingsSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _countSavings, _countMovements, _countIncomes, _sourceAccountId, _oldTargetAccountId, _newTargetAccountId, _savingId;
        private decimal _sourceAccountAmount, _oldTargetAccountAmount, _newTargetAccountAmount, _costSaving, _newCostSaving;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly ISavingService _savingService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;

        public EditSavingsSteps()
        {
            _bankAccountService = new BankAccountService();
            _savingService = new SavingService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
        }

        [Given(@"I have accessed the Saving List page")]
        public void GivenIHaveAccessedTheSavingListPage()
        {
            _ctx.GotToUrl("/Saving/Index");

            // Get Source Account Amount Before Creating Savings
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            // Get Number Of Savings Before Creating Savings
            _countSavings = _savingService.CountSavings();

            // Get Number Of Incomes Before Creating Savings
            _countIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Savings
            _countMovements = _historicMovementService.CountMovements();
        }
        
        [Given(@"I have at least one saving in the list")]
        public void GivenIHaveAtLeastOneSavingInTheList()
        {
            var savings = _ctx.WebDriver.FindElements(By.ClassName("trSaving"));
            if (savings.Count < 1)
            {
                throw new Exception("There is no saving to delete");
            }
            _firstRow = savings[0];
        }
        
        [When(@"I click on edit for the first saving")]
        public void WhenIClickOnEditForTheFirstSaving()
        {
            var costValue = _firstRow.FindElement(By.ClassName("tdAmount"));
            _costSaving = Convert.ToDecimal(costValue.Text.Substring(1));

            var targetAccountHid = _firstRow.FindElement(By.Id("item_TargetInternalAccountId"));
            _oldTargetAccountId = Convert.ToInt32(targetAccountHid.GetAttribute("value"));
            _oldTargetAccountAmount = _bankAccountService.GetAccountAmount(_oldTargetAccountId);

            var editConfirmBtn = _firstRow.FindElement(By.ClassName("btn_edit"));
            editConfirmBtn.Click();

            var savingIdHid = _ctx.WebDriver.FindElement(By.Id("Id"));
            _savingId = Convert.ToInt32(savingIdHid.GetAttribute("value"));
        }
        
        [When(@"I edit the Amount")]
        public void WhenIEditTheAmount()
        {
            var amountTxt = _ctx.WebDriver.FindElement(By.Id("Amount"));
            amountTxt.Clear();
            amountTxt.SendKeys((_costSaving+100).ToString());
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }
        
        [When(@"I select another account")]
        public void WhenISelectAnotherAccount()
        {
            var accountDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            if (accountDdl.Options.Count < 3)
                throw new Exception("TargetInternalAccountId has no enough option. At least 2 expected.");

            var selectedValueOption = Convert.ToInt32(accountDdl.AllSelectedOptions[0].GetAttribute("value"));

            var savingAccounts = new List<Tuple<int,int,bool>>();
            for(var ind = 1; ind < accountDdl.Options.Count; ind++)
            {
                var value = Convert.ToInt32(accountDdl.Options[ind].GetAttribute("value"));
                savingAccounts.Add(new Tuple<int, int, bool>(ind, value, value == selectedValueOption));
            }

            var firstNotSelectedIndex = savingAccounts.Where(x => !x.Item3).First();

            accountDdl.SelectByIndex(firstNotSelectedIndex.Item1);

            // Get New Target Account Amount Before Creating Savings
            _newTargetAccountId = firstNotSelectedIndex.Item2;
            _newTargetAccountAmount = _bankAccountService.GetAccountAmount(_newTargetAccountId);
        }
        
        [Then(@"the Saving has been updated")]
        public void ThenTheSavingHasBeenUpdated()
        {
            // Get Number Of Savings After
            var newCountSavings = _savingService.CountSavings();
            Assert.AreEqual(newCountSavings, _countSavings);

            _newCostSaving = _savingService.GetSavingCost(_savingId);
            Assert.AreEqual(_costSaving + 100, _newCostSaving);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            var expectedSourceAmount = _sourceAccountAmount + _costSaving - _newCostSaving;

            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }
        
        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_oldTargetAccountId);

            var expectedTargetAmount = _oldTargetAccountAmount - _costSaving + _newCostSaving;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the old target account is updated")]
        public void ThenTheOldTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_oldTargetAccountId);

            var expectedTargetAmount = _oldTargetAccountAmount - _costSaving;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the new target account is updated")]
        public void ThenTheNewTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_newTargetAccountId);

            var expectedTargetAmount = _newTargetAccountAmount + _newCostSaving;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
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
    }
}
