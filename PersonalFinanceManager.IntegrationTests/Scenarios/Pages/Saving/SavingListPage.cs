using System;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Saving
{
    public class SavingListPage : Infrastructure.BasePages.ListPage
    {
        public override string PageUrl => "/Saving/Index";

        public override string DeletionConfirmationModalTitle => "Delete a saving";
        public override string RowClassName => "trSaving";

        private const string CostColClassName = "tdAmount";
        private const string TargetSavingAccountPropertyId = "item_TargetInternalAccountId";

        public SavingListPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public decimal FindCost(IWebElement firstRow)
        {
            var elm = firstRow.FindElement(By.ClassName(CostColClassName));
            return Convert.ToDecimal(elm.Text.Substring(1));
        }

        public int FindTargetInternalAccountId(IWebElement firstRow)
        {
            var elm = firstRow.FindElement(By.Id(TargetSavingAccountPropertyId));
            return Convert.ToInt32(elm.GetAttribute("value"));
        }
    }
}
