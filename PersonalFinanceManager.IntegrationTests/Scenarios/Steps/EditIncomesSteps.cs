using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditIncomes")]
    public class EditIncomesSteps
    {
        public IntegrationTestContext ctx = new IntegrationTestContext();

        public int SourceAccountId, IncomeId;
        public int CountIncomes, CountMovements;
        public decimal SourceAccountAmount;
        public decimal CostIncome, NewCostIncome;

        public IWebElement FirstRow;

        public IBankAccountService _bankAccountService;
        public IIncomeService _incomeService;
        public IHistoricMovementService _historicMovementService;

        public EditIncomesSteps()
        {
            _bankAccountService = new BankAccountService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
        }

        [Given(@"I have accessed the Income List page")]
        public void GivenIHaveAccessedTheIncomeListPage()
        {
            ctx.GotToUrl("/Income/Index");

            // Get Source Account Amount Before Creating Savings
            SourceAccountId = ctx.SelectedSourceAccountId();
            SourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            // Get Number Of Incomes Before Creating Savings
            CountIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Savings
            CountMovements = _historicMovementService.CountMovements();
        }

        [Given(@"I have at least one income in the list")]
        public void GivenIHaveAtLeastOneIncomeInTheList()
        {
            var incomes = ctx.WebDriver.FindElements(By.ClassName("trIncome"));
            if (incomes.Count < 1)
            {
                throw new Exception("There is no income to delete");
            }
            FirstRow = incomes[0];
        }
        
        [When(@"I click on edit for the first income")]
        public void WhenIClickOnEditForTheFirstIncome()
        {
            var costValue = FirstRow.FindElement(By.ClassName("tdCost"));
            CostIncome = Convert.ToDecimal(costValue.Text.Substring(1));

            var editConfirmBtn = FirstRow.FindElement(By.ClassName("btn_edit"));
            editConfirmBtn.Click();

            var incomeIdHid = ctx.WebDriver.FindElement(By.Id("Id"));
            IncomeId = Convert.ToInt32(incomeIdHid.GetAttribute("value"));
        }
        
        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            var costTxt = ctx.WebDriver.FindElement(By.Id("Cost"));
            costTxt.Clear();
            costTxt.SendKeys((CostIncome + 100).ToString());
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }

        [Then(@"the Income has been updated")]
        public void ThenTheIncomeHasBeenUpdated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();
            Assert.AreEqual(newCountIncomes, CountIncomes);

            NewCostIncome = _incomeService.GetIncomeCost(IncomeId);
            Assert.AreEqual(CostIncome + 100, NewCostIncome);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            var expectedSourceAmount = SourceAccountAmount - CostIncome + NewCostIncome;

            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
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
