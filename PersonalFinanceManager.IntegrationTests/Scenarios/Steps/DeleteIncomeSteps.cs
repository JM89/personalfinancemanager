using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteIncomes")]
    public class DeleteIncomesSteps
    {
        public IntegrationTestContext ctx = new IntegrationTestContext();
        public int CountMovements, CountIncomes;
        public int SourceAccountId;
        public decimal SourceAccountAmount, TargetAccountAmount;
        public decimal CostIncome;

        public IWebElement FirstRow;

        public IBankAccountService _bankAccountService;
        public IIncomeService _incomeService;
        public IHistoricMovementService _historicMovementService;

        public DeleteIncomesSteps()
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

        [When(@"I click on delete for the first income")]
        public void WhenIClickOnDeleteForTheFirstIncome()
        {
            var costValue = FirstRow.FindElement(By.ClassName("tdCost"));
            CostIncome = Convert.ToDecimal(costValue.Text.Substring(1));
            
            var deleteConfirmBtn = FirstRow.FindElement(By.ClassName("btn_delete"));
            deleteConfirmBtn.Click();
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            var deleteIncomePage = ctx.WebDriver.FindElement(By.TagName("h5"));
            if (deleteIncomePage.Text != "Delete an income")
            {
                throw new Exception("The confirmation of deletion should be there.");
            }
            var deleteBtn = ctx.WebDriver.FindElement(By.ClassName("btn_delete"));
            deleteBtn.Click();
        }

        [Then(@"the income has been removed")]
        public void ThenTheIncomeHasBeenRemoved()
        {
            // Get Number Of Savings After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, CountIncomes - 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, SourceAccountAmount - 100);
        }
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, CountMovements + 1);
        }
    }
}
