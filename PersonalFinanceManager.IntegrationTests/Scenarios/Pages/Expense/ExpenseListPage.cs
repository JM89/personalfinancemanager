using System;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Expense
{
    public class ExpenseListPage : Infrastructure.BasePages.ListPage
    {
        public override string PageUrl => "/Expenditure/Index";

        public override string DeletionConfirmationModalTitle => "Delete an expense";
        public override string RowClassName => "trExpenditure";

        private const string PaymentMethodColClassName = "tdPaymentMethod";
        private const string CostColClassName = "tdCost";
        private const string PaymentMethodPropertyId = "item_PaymentMethodName";
        private const string AtmWithdrawPropertyId = "AtmWithdrawId";
        private const string TargetInternalAccountPropertyId = "TargetInternalAccountId";

        public ExpenseListPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public IWebElement FindFirstRowAndCheckPaymentMethod(string text)
        {
            var row = FindFirstRow();
            var paymentMethod = row.FindElement(By.ClassName(PaymentMethodColClassName)).FindElement(By.Id(PaymentMethodPropertyId)).GetAttribute("value");
            if (paymentMethod != text)
            {
                throw new Exception($"There is no expenditure with payment method {text} to delete");
            }
            return row;
        }

        public int FindAtmWithdrawId(IWebElement firstRow)
        {
            var elm = firstRow.FindElement(By.Id(AtmWithdrawPropertyId));
            return Convert.ToInt32(elm.GetAttribute("value"));
        }

        public decimal FindCost(IWebElement firstRow)
        {
            var elm = firstRow.FindElement(By.ClassName(CostColClassName));
            return Convert.ToDecimal(elm.Text.Substring(1));
        }

        public int FindTargetInternalAccountId(IWebElement firstRow)
        {
            var elm = firstRow.FindElement(By.Id(TargetInternalAccountPropertyId));
            return Convert.ToInt32(elm.GetAttribute("value"));
        }
    }
}
