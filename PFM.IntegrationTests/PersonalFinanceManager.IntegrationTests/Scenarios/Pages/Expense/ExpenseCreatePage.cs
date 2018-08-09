using System;
using System.Globalization;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Expense
{
    public class ExpenseCreatePage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Expenditure/Create";

        private const string CostPropertyId = "Cost";
        private const string DescriptionPropertyId = "Description";
        private const string ExpenseTypePropertyId = "TypeExpenditureId";
        private const string PaymentMethodPropertyId = "paymentMethodId";
        private const string AtmWithdrawPropertyId = "AtmWithdrawId";
        private const string TargetInternalPropertyId = "TargetInternalAccountId";

        public ExpenseCreatePage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void QuickCreateCommon()
        {
            GoTo();
            EnterDescription("CB");
            EnterCost(100);
            SelectFirstExpenseType();
            SelectPaymentMethod("CB");
            ClickSave();
        }

        public void QuickCreateInternalTransfer()
        {
            GoTo();
            EnterDescription("Internal Transfer");
            EnterCost(100);
            SelectFirstExpenseType();
            SelectPaymentMethod("Internal Transfer");
            SelectFirstTargetAccount();
            ClickSave();
        }

        public void QuickCreateCash()
        {
            GoTo();
            EnterDescription("Cash");
            EnterCost(100);
            SelectFirstExpenseType();
            SelectPaymentMethod("Cash");
            SelectFirstAtmWithdraw();
            ClickSave();
        }

        public void EnterCost(decimal cost)
        {
            var field = WebDriver.FindElement(By.Id(CostPropertyId));
            field.Clear();
            field.SendKeys(cost.ToString(CultureInfo.InvariantCulture));
        }

        public void EnterDescription(string text)
        {
            var field = WebDriver.FindElement(By.Id(DescriptionPropertyId));
            field.Clear();
            field.SendKeys(text);
        }

        public void SelectFirstExpenseType()
        {
            var expenditureTypeDdl = new SelectElement(WebDriver.FindElement(By.Id(ExpenseTypePropertyId)));
            if (expenditureTypeDdl.Options.Count < 2)
                throw new Exception("TypeExpenditureId has no option. At least 1 expected.");
            expenditureTypeDdl.SelectByIndex(1);
        }

        public void SelectPaymentMethod(string value)
        {
            var paymentMethodDdl = new SelectElement(WebDriver.FindElement(By.Id(PaymentMethodPropertyId)));
            paymentMethodDdl.SelectByText(value);
            Thread.Sleep(2000);
        }

        public int SelectFirstAtmWithdraw()
        {
            var atmWithdrawDdl = new SelectElement(WebDriver.FindElement(By.Id(AtmWithdrawPropertyId)));
            if (atmWithdrawDdl.Options.Count < 2)
                throw new Exception("AtmWithdrawId has no enough option. At least 1 expected.");
            atmWithdrawDdl.SelectByIndex(1);

            return Convert.ToInt32(atmWithdrawDdl.Options[1].GetAttribute("value"));
        }

        public int SelectFirstTargetAccount()
        {
            var accountDdl = new SelectElement(WebDriver.FindElement(By.Id(TargetInternalPropertyId)));
            if (accountDdl.Options.Count < 2)
                throw new Exception("TargetInternalAccountId has no option. At least 1 expected.");
            accountDdl.SelectByIndex(1);

            return Convert.ToInt32(accountDdl.Options[1].GetAttribute("value"));
        }
    }
}
