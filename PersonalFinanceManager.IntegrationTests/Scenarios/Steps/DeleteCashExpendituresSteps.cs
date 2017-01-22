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
    [Binding, Scope(Feature = "DeleteCashExpenditures")]
    public class DeleteCashExpendituresSteps
    {
        private readonly IntegrationTestContext _ctx = new IntegrationTestContext();

        private int _sourceAccountId, _countExpenditures, _countMovements, _atmWithdrawId;
        private decimal _sourceAccountAmount, _costExpenditure, _atmWithdrawAmount;
        private IWebElement _firstRow;

        private readonly IBankAccountService _bankAccountService;
        private readonly IAtmWithdrawService _atmWithdrawService;
        private readonly IHistoricMovementService _historicMovementService;
        private readonly IExpenditureService _expenditureService;

        public DeleteCashExpendituresSteps()
        {
            _bankAccountService = new BankAccountService();
            _historicMovementService = new HistoricMovementService();
            _expenditureService = new ExpenditureService();
            _atmWithdrawService = new AtmWithdrawService();
        }

        [BeforeScenario]
        public void PrepareForTest()
        {
            CreateCashExpenditures.Execute(_ctx);
        }

        [Given(@"I have accessed the Expenditures List page")]
        public void GivenIHaveAccessedTheExpendituresListPage()
        {
            _ctx.GotToUrl("/Expenditure/Index");

            // Get Source Account Amount Before Creating Expenditures
            _sourceAccountId = _ctx.SelectedSourceAccountId();
            _sourceAccountAmount = _bankAccountService.GetAccountAmount(_sourceAccountId);

            // Get Number Of Savings Before Creating Expenditures
            _countExpenditures = _expenditureService.CountExpenditures();

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

            var paymentMethod = _firstRow.FindElement(By.ClassName("tdPaymentMethod")).Text;
            if (paymentMethod != "Cash")
            {
                throw new Exception("There is no expenditure with payment method Cash to delete");
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
            if (deleteExpenditurePage.Text != "Delete an expenditure")
            {
                throw new Exception("The confirmation of deletion should be there.");
            }

            var atmWithdrawHid = _ctx.WebDriver.FindElement(By.Id("AtmWithdrawId"));
            _atmWithdrawId = Convert.ToInt32(atmWithdrawHid.GetAttribute("value"));
            _atmWithdrawAmount = _atmWithdrawService.GetAtmWithdrawCurrentAmount(_atmWithdrawId);

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
        
        [Then(@"a mouvement entry has been saved")]
        public void ThenAMouvementEntryHasBeenSaved()
        {
            // Get Number Of Movements After
            var newCountMovements = _historicMovementService.CountMovements();

            Assert.AreEqual(newCountMovements, _countMovements + 1);
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

            var expectedTargetAmount = _atmWithdrawAmount + 100;

            Assert.AreEqual(expectedTargetAmount, newTargetAtmWithdrawAmount);
        }

        [AfterScenario]
        public void TestTearDown()
        {
            _ctx.StopTest();
        }
    }
}
