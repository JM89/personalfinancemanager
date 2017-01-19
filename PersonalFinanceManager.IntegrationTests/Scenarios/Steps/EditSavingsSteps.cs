using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditSavings")]
    public class EditSavingsSteps
    {
        public IntegrationTestContext ctx = new IntegrationTestContext();
        public int CountSavings, CountMovements, CountIncomes;
        public int SourceAccountId, OldTargetAccountId, NewTargetAccountId, SavingId;
        public decimal SourceAccountAmount, OldTargetAccountAmount, NewTargetAccountAmount;
        public decimal CostSaving, NewCostSaving;

        public IWebElement FirstRow;

        public IBankAccountService _bankAccountService;
        public ISavingService _savingService;
        public IIncomeService _incomeService;
        public IHistoricMovementService _historicMovementService;

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
            ctx.GotToUrl("/Saving/Index");

            // Get Source Account Amount Before Creating Savings
            SourceAccountId = ctx.SelectedSourceAccountId();
            SourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            // Get Number Of Savings Before Creating Savings
            CountSavings = _savingService.CountSavings();

            // Get Number Of Incomes Before Creating Savings
            CountIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Savings
            CountMovements = _historicMovementService.CountMovements();
        }
        
        [Given(@"I have at least one saving in the list")]
        public void GivenIHaveAtLeastOneSavingInTheList()
        {
            FirstRow = ctx.WebDriver.FindElement(By.Id("row-1"));
            if (FirstRow == null)
            {
                throw new Exception("There is no saving to delete");
            }
        }
        
        [When(@"I click on edit for the first saving")]
        public void WhenIClickOnEditForTheFirstSaving()
        {
            var costValue = FirstRow.FindElement(By.ClassName("tdAmount"));
            CostSaving = Convert.ToDecimal(costValue.Text.Substring(1));

            var targetAccountHid = FirstRow.FindElement(By.Id("item_TargetInternalAccountId"));
            OldTargetAccountId = Convert.ToInt32(targetAccountHid.GetAttribute("value"));
            OldTargetAccountAmount = _bankAccountService.GetAccountAmount(OldTargetAccountId);

            var editConfirmBtn = FirstRow.FindElement(By.ClassName("btn_edit"));
            editConfirmBtn.Click();

            var savingIdHid = ctx.WebDriver.FindElement(By.Id("Id"));
            SavingId = Convert.ToInt32(savingIdHid.GetAttribute("value"));
        }
        
        [When(@"I edit the Amount")]
        public void WhenIEditTheAmount()
        {
            var amountTxt = ctx.WebDriver.FindElement(By.Id("Amount"));
            amountTxt.Clear();
            amountTxt.SendKeys((CostSaving+100).ToString());
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();
        }
        
        [When(@"I select another account")]
        public void WhenISelectAnotherAccount()
        {
            var accountDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
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
            NewTargetAccountId = firstNotSelectedIndex.Item2;
            NewTargetAccountAmount = _bankAccountService.GetAccountAmount(NewTargetAccountId);
        }
        
        [Then(@"the Saving has been updated")]
        public void ThenTheSavingHasBeenUpdated()
        {
            // Get Number Of Savings After
            var newCountSavings = _savingService.CountSavings();
            Assert.AreEqual(newCountSavings, CountSavings);

            NewCostSaving = _savingService.GetSavingCost(SavingId);
            Assert.AreEqual(CostSaving + 100, NewCostSaving);
        }
        
        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            var expectedSourceAmount = SourceAccountAmount + CostSaving - NewCostSaving;

            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
        }
        
        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(OldTargetAccountId);

            var expectedTargetAmount = OldTargetAccountAmount - CostSaving + NewCostSaving;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the old target account is updated")]
        public void ThenTheOldTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(OldTargetAccountId);

            var expectedTargetAmount = OldTargetAccountAmount - CostSaving;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"the new target account is updated")]
        public void ThenTheNewTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(NewTargetAccountId);

            var expectedTargetAmount = NewTargetAccountAmount + NewCostSaving;

            Assert.AreEqual(expectedTargetAmount, newTargetAccountAmount);
        }

        [Then(@"an income has been updated")]
        public void ThenAnIncomeHasBeenUpdated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, CountIncomes);
        }
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, CountMovements + 2);
        }
    }
}
