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
    [Binding, Scope(Feature = "EditIncomes")]
    public class EditIncomesSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _sourceAccountId, _incomeId, _countIncomes, _countMovements;
        private decimal _sourceAccountAmount, _costIncome, _newCostIncome;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;

        public EditIncomesSteps()
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
            // Get Source Account Amount Before Creating Incomes
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            _ctx.GotToUrl("/Income/Index");

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
        
        [When(@"I click on edit for the first income")]
        public void WhenIClickOnEditForTheFirstIncome()
        {
            var costValue = _firstRow.FindElement(By.ClassName("tdCost"));
            _costIncome = Convert.ToDecimal(costValue.Text.Substring(1));

            var editBtn = _firstRow.FindElement(By.ClassName("btn_edit"));
            editBtn.Click();

            var incomeIdHid = _ctx.WebDriver.FindElement(By.Id("Id"));
            _incomeId = Convert.ToInt32(incomeIdHid.GetAttribute("value"));
        }
        
        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            var costTxt = _ctx.WebDriver.FindElement(By.Id("Cost"));
            costTxt.Clear();
            costTxt.SendKeys((_costIncome + 100).ToString());
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }

        [Then(@"the Income has been updated")]
        public void ThenTheIncomeHasBeenUpdated()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();
            Assert.AreEqual(newCountIncomes, _countIncomes);

            _newCostIncome = _incomeService.GetIncomeCost(_incomeId);
            Assert.AreEqual(_costIncome + 100, _newCostIncome);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            var expectedSourceAmount = _sourceAccountAmount - _costIncome + _newCostIncome;

            Assert.AreEqual(expectedSourceAmount, newSourceAccountAmount);
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
