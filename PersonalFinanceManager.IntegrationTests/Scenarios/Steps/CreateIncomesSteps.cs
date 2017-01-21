using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Threading;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateIncomes")]
    public class CreateIncomesSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _sourceAccountId, _countIncomes, _countMovements;
        private decimal _sourceAccountAmount;

        private readonly IBankAccountService _bankAccountService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;

        public CreateIncomesSteps()
        {
            _bankAccountService = new BankAccountService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
        }
        
        [Given(@"I have accessed the Income List page")]
        public void GivenIHaveAccessedTheIncomeListPage()
        {
            _ctx.GotToUrl("/Income/Index");

            // Get Source Account Amount Before Creating Incomes
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            // Get Number Of Incomes Before Creating Incomes
            _countIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Incomes
            _countMovements = _historicMovementService.CountMovements();
        }

        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            var createBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_create"));
            createBtn.Click();
        }
        
        [When(@"I enter a description")]
        public void WhenIEnterADescription()
        {
            var descriptionTxt = _ctx.WebDriver.FindElement(By.Id("Description"));
            descriptionTxt.Clear();
            descriptionTxt.SendKeys("Income Description");
        }
        
        [When(@"I enter Cost")]
        public void WhenIEnterCost()
        {
            var costTxt = _ctx.WebDriver.FindElement(By.Id("Cost"));
            costTxt.Clear();
            costTxt.SendKeys("100.00");
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + 100);
        }
        
        [Then(@"an income has been created")]
        public void ThenAnIncomeHasBeenCreated()
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

            Assert.AreEqual(newCountMovements, _countMovements + 1);
        }

        [AfterScenario]
        public void TestTearDown()
        {
            _ctx.StopTest();
        }
    }
}
