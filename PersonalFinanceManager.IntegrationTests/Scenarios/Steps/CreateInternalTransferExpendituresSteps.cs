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
    [Binding, Scope(Feature = "CreateInternalTransferExpenditures")]
    public class CreateInternalTransferExpendituresSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();
        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount;

        private readonly IBankAccountService _bankAccountService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;
        private readonly IExpenditureService _expenditureService;

        public CreateInternalTransferExpendituresSteps()
        {
            _bankAccountService = new BankAccountService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
            _expenditureService = new ExpenditureService();
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

            // Get Number Of Incomes Before Creating Expenditures
            _countIncomes = _incomeService.CountIncomes();

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

        [When(@"I select the Internal Transfer payment type")]
        public void WhenISelectTheInternalTransferPaymentType()
        {
            var paymentMethodDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("paymentMethodId")));
            paymentMethodDdl.SelectByText("Internal Transfer");

            Thread.Sleep(2000);
        }
        
        [When(@"I select the first target account")]
        public void WhenISelectTheFirstTargetAccount()
        {
            var accountDdl = new SelectElement(_ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            if (accountDdl.Options.Count < 2)
                throw new Exception("TargetInternalAccountId has no option. At least 1 expected.");
            accountDdl.SelectByIndex(1);

            // Get Target Account Amount Before Creating Expenditures
            _targetAccountId = Convert.ToInt32(accountDdl.Options[1].GetAttribute("value"));
            _targetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);
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

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount - 100);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);

            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount + 100);
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
