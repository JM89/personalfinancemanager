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
    [Binding, Scope(Feature = "DeleteInternalTransferExpenditures")]
    public class DeleteInternalTransferExpendituresSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _sourceAccountId, _countExpenditures, _countIncomes, _countMovements, _targetAccountId;
        private decimal _sourceAccountAmount, _targetAccountAmount, _costExpenditure;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly IIncomeService _incomeService;
        private readonly IHistoricMovementService _historicMovementService;
        private readonly IExpenditureService _expenditureService;

        public DeleteInternalTransferExpendituresSteps()
        {
            _bankAccountService = new BankAccountService();
            _incomeService = new IncomeService();
            _historicMovementService = new HistoricMovementService();
            _expenditureService = new ExpenditureService();
        }
        
        [BeforeScenario]
        public void PrepareForTest()
        {
            CreateInternalTransferExpenditures.Execute(_ctx);
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            // Get Source Account Amount Before Creating Expenditures
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            _ctx.GotToUrl("/Expenditure/Index");

            // Get Number Of Savings Before Creating Expenditures
            _countExpenditures = _expenditureService.CountExpenditures();

            // Get Number Of Incomes Before Creating Expenditures
            _countIncomes = _incomeService.CountIncomes();

            // Get Number Of Movements Before Creating Expenditures
            _countMovements = _historicMovementService.CountMovements();
        }
        
        [Given(@"I have at least one expenditure with this payment method in the list")]
        public void GivenIHaveAtLeastOneExpenditureWithThisPaymentMethodInTheList()
        {
            var expenditures = _ctx.WebDriver.FindElements(By.ClassName("trExpenditure"));
            if (expenditures.Count < 1)
            {
                throw new Exception("There is no expenditure to delete");
            }

            _firstRow = expenditures[0];

            var paymentMethod = _firstRow.FindElement(By.ClassName("tdPaymentMethod")).FindElement(By.Id("item_PaymentMethodName")).GetAttribute("value");
            if (paymentMethod != "Internal Transfer")
            {
                throw new Exception("There is no expenditure with payment method Internal Transfer to delete");
            }           
        }
        
        [When(@"I click on delete for this expenditure")]
        public void WhenIClickOnDeleteForThisExpenditure()
        {
            var costValue = _firstRow.FindElement(By.ClassName("tdCost"));
            _costExpenditure = Convert.ToDecimal(costValue.Text.Substring(1));

            var deleteConfirmBtn = _firstRow.FindElement(By.ClassName("btn_delete"));
            deleteConfirmBtn.Click();
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            var deleteExpenditurePage = _ctx.WebDriver.FindElement(By.TagName("h5"));
            if (deleteExpenditurePage.Text != "Delete an expense")
            {
                throw new Exception("The confirmation of deletion should be there.");
            }
            
            var targetAccountHid = _ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId"));
            _targetAccountId = Convert.ToInt32(targetAccountHid.GetAttribute("value"));
            _targetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);

            var deleteBtn = _ctx.WebDriver.FindElement(By.ClassName("btn_delete"));
            deleteBtn.Click();

            Thread.Sleep(2000);
        }

        [Then(@"the expenditure has been removed")]
        public void ThenTheExpenditureHasBeenRemoved()
        {
            // Get Number Of Expenditures After
            var newCountExpenditures = _expenditureService.CountExpenditures();

            Assert.AreEqual(newCountExpenditures, _countExpenditures - 1);
        }

        [Then(@"the source account is updated")]
        public void ThenTheSourceAccountIsUpdated()
        {
            // Get Source Account Amount After
            var newSourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            Assert.AreEqual(newSourceAccountAmount, _sourceAccountAmount + _costExpenditure);
        }

        [Then(@"the target account is updated")]
        public void ThenTheTargetAccountIsUpdated()
        {
            // Get Target Account Amount After
            var newTargetAccountAmount = _bankAccountService.GetAccountAmount(_targetAccountId);

            Assert.AreEqual(newTargetAccountAmount, _targetAccountAmount - _costExpenditure);
        }

        [Then(@"an income has been removed")]
        public void ThenAnIncomeHasBeenRemoved()
        {
            // Get Number Of Incomes After
            var newCountIncomes = _incomeService.CountIncomes();

            Assert.AreEqual(newCountIncomes, _countIncomes - 1);
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
