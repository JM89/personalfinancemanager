using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using PersonalFinanceManager.ServicesForTests;
using PersonalFinanceManager.ServicesForTests.Interfaces;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding, Scope(Feature = "CreateCashExpenditures")]
    public class CreateCashExpendituresSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();
        private int _sourceAccountId, _countExpenditures, _countMovements, _atmWithdrawId;
        private decimal _sourceAccountAmount, _atmWithdrawAmount;

        private readonly IBankAccountService _bankAccountService;
        private readonly IHistoricMovementService _historicMovementService;
        private readonly IExpenditureService _expenditureService;
        private readonly IAtmWithdrawService _atmWithdrawService;

        public CreateCashExpendituresSteps()
        {
            _bankAccountService = new BankAccountService();
            _historicMovementService = new HistoricMovementService();
            _expenditureService = new ExpenditureService();
            _atmWithdrawService = new AtmWithdrawService();
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            // Get Source Account Amount Before Creating Expenditures
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            _ctx.GotToUrl("/Expenditure/Index");

            // Get Number Of Expenditures Before Creating Expenditures
            _countExpenditures = _expenditureService.CountExpenditures();

            // Get Number Of Movements Before Creating Expenditures
            _countMovements = _historicMovementService.CountMovements();
        }

        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            var createBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_create"));
            createBtn.Click();
        }

        [When(@"I enter Description")]
        public void WhenIEnterDescription()
        {
            var descriptionTxt = _ctx.WebDriver.FindElement(By.Id("Description"));
            descriptionTxt.Clear();
            descriptionTxt.SendKeys("Internal Transfer");
        }

        [When(@"I enter a Cost")]
        public void WhenIEnterACost()
        {
            var costTxt = _ctx.WebDriver.FindElement(By.Id("Cost"));
            costTxt.Clear();
            costTxt.SendKeys("100.00");
        }

        [When(@"I select the first expenditure type")]
        public void WhenISelectTheFirstExpenditureType()
        {
            var expenditureTypeDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("TypeExpenditureId")));
            if (expenditureTypeDdl.Options.Count < 2)
                throw new Exception("TypeExpenditureId has no option. At least 1 expected.");
            expenditureTypeDdl.SelectByIndex(1);
        }

        [When(@"I select the Cash payment type")]
        public void WhenISelectTheCashPaymentType()
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

        [Then(@"the Expenditure Has Been Created")]
        public void ThenTheExpenditureHasBeenCreated()
        {
            // Get Number Of Expenditures After
            var newCountExpenditures = _expenditureService.CountExpenditures();

            Assert.AreEqual(newCountExpenditures, _countExpenditures + 1);
        }

        [Then(@"the source account is unchanged")]
        public void ThenTheSourceAccountUnchanged()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount);
        }

        [Then(@"the target atm withdraw is updated")]
        public void ThenTheTargetAtmWithdrawIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAtmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_atmWithdrawId);

            var expectedTargetAmount = _atmWithdrawAmount - 100;

            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
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
