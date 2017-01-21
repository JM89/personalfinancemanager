using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateIncomes")]
    public class CreateIncomesSteps
    {
        public IntegrationTestContext ctx = new IntegrationTestContext();

        public int SourceAccountId;
        public int CountIncomes, CountMovements;
        public decimal SourceAccountAmount;

        public IBankAccountService _bankAccountService;
        public IIncomeService _incomeService;
        public IHistoricMovementService _historicMovementService;

        public CreateIncomesSteps()
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

        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            var createBtn = ctx.WebDriver.FindElement(By.ClassName("btn_create"));
            createBtn.Click();
        }
        
        [When(@"I enter a description")]
        public void WhenIEnterADescription()
        {
            var descriptionTxt = ctx.WebDriver.FindElement(By.Id("Description"));
            descriptionTxt.Clear();
            descriptionTxt.SendKeys("Income Description");
        }
        
        [When(@"I enter Cost")]
        public void WhenIEnterCost()
        {
            var costTxt = ctx.WebDriver.FindElement(By.Id("Cost"));
            costTxt.Clear();
            costTxt.SendKeys("100.00");
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(SourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, SourceAccountAmount + 100);
        }
        
        [Then(@"an income has been created")]
        public void ThenAnIncomeHasBeenCreated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, CountIncomes + 1);
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
