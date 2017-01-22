using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.IntegrationTests.Scenarios.PreActions;
using PersonalFinanceManager.ServicesForTests;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "EditAtmWithdraws")]
    public class EditAtmWithdrawsSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _sourceAccountId, _atmWithdrawId, _countAtmWithdraws, _countMovements;
        private decimal _sourceAccountAmount, _costAtmWithdraw, _newCostAtmWithdraw;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly IAtmWithdrawService _atmWithdrawService;
        private readonly IHistoricMovementService _historicMovementService;

        public EditAtmWithdrawsSteps()
        {
            _bankAccountService = new BankAccountService();
            _atmWithdrawService = new AtmWithdrawService();
            _historicMovementService = new HistoricMovementService();
        }

        [BeforeScenario]
        public void PrepareForTest()
        {
            CreateAtmWithdraws.Execute(_ctx);
        }

        [Given(@"I have accessed the ATM Withdraw List page")]
        public void GivenIHaveAccessedTheAtmWithdrawListPage()
        {
            _ctx.GotToUrl("/AtmWithdraw/Index");

            // Get Source Account Amount Before Creating AtmWithdraws
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            // Get Number Of Incomes Before Creating AtmWithdraws
            _countAtmWithdraws = _atmWithdrawService.CountAtmWithdraws();

            // Get Number Of Movements Before Creating AtmWithdraws
            _countMovements = _historicMovementService.CountMovements();
        }

        [Given(@"I have at least one ATM withdraw in the list")]
        public void GivenIHaveAtLeastOneAtmWithdrawInTheList()
        {
            var atmWithdraws = _ctx.WebDriver.FindElements(By.ClassName("trAtmWithdraw"));
            if (atmWithdraws.Count < 1)
            {
                throw new Exception("There is no atm withdraws to delete");
            }
            _firstRow = atmWithdraws[0];
        }
        
        [When(@"I click on edit for the first ATM withdraw")]
        public void WhenIClickOnEditForTheFirstAtmWithdraw()
        {
            var costValue = _firstRow.FindElement(By.ClassName("tdInitialAmount"));
            _costAtmWithdraw = Convert.ToDecimal(costValue.Text.Substring(1));

            var editBtn = _firstRow.FindElement(By.ClassName("btn_edit"));
            editBtn.Click();
            
            var atmWithdrawIdHid = _ctx.WebDriver.FindElement(By.Id("Id"));
            _atmWithdrawId = Convert.ToInt32(atmWithdrawIdHid.GetAttribute("value"));
        }
        
        [When(@"I edit the Cost")]
        public void WhenIEditTheCost()
        {
            var initialAmountTxt = _ctx.WebDriver.FindElement(By.Id("InitialAmount"));
            initialAmountTxt.Clear();
            initialAmountTxt.SendKeys((_costAtmWithdraw + 100).ToString());
        }

        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            var saveBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }
        
        [Then(@"the ATM withdraw has been updated")]
        public void ThenTheAtmWithdrawHasBeenUpdated()
        {
            // Get Number Of ATM Withdraws After
            var newCountAtmWithdraws = _atmWithdrawService.CountAtmWithdraws();
            Assert.AreEqual(newCountAtmWithdraws, _countAtmWithdraws);

            _newCostAtmWithdraw = _atmWithdrawService.GetAtmWithdrawInitialAmount(_atmWithdrawId);
            Assert.AreEqual(_costAtmWithdraw + 100, _newCostAtmWithdraw);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            var expectedSourceAmount = _sourceAccountAmount + _costAtmWithdraw - _newCostAtmWithdraw;

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
