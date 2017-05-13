using System;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.AtmWithdraw
{
    public class AtmWithdrawListPage : Infrastructure.BasePages.ListPage
    {
        public override string PageUrl => "/AtmWithdraw/Index";

        public override string RowClassName => "trAtmWithdraw";
        public override string DeletionConfirmationModalTitle => "Delete an ATM withdraw";

        private const string CostColClassName = "tdInitialAmount";

        public AtmWithdrawListPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public decimal FindCost(IWebElement firstRow)
        {
            var costValue = firstRow.FindElement(By.ClassName(CostColClassName));
            return Convert.ToDecimal(costValue.Text.Substring(1));
        }
    }
}
