using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using System;
using System.Threading;
using PersonalFinanceManager.IntegrationTests.Scenarios.PreActions;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "DeleteIncomes")]
    public class DeleteIncomesSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _countMovements, _countIncomes, _sourceAccountId;
        private decimal _sourceAccountAmount;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;

        public DeleteIncomesSteps()
        {
            _bankAccountService = new BankAccountService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
        }

        [BeforeScenario]
        public void PrepareForTest()
        {
            CreateIncomes.Execute(_ctx);
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


        [Given(@"I have at least one income in the list")]
        public void GivenIHaveAtLeastOneIncomeInTheList()
        {
            var incomes = _ctx.WebDriver.FindElements(By.ClassName("trIncome"));
            if (incomes.Count < 1)
            {
                throw new Exception("There is no income to delete");
            }
            _firstRow = incomes[0];
        }

        [When(@"I click on delete for the first income")]
        public void WhenIClickOnDeleteForTheFirstIncome()
        {
            var deleteConfirmBtn = _firstRow.FindElement(By.ClassName("btn_delete"));
            deleteConfirmBtn.Click();
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            var deleteIncomePage = _ctx.WebDriver.FindElement(By.TagName("h5"));
            if (deleteIncomePage.Text != "Delete an income")
            {
                throw new Exception("The confirmation of deletion should be there.");
            }
            var deleteBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_delete"));
            deleteBtn.Click();

            Thread.Sleep(2000);
        }

        [Then(@"the income has been removed")]
        public void ThenTheIncomeHasBeenRemoved()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
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
