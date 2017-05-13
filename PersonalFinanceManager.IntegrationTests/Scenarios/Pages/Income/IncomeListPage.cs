using System;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Income
{
    public class IncomeListPage : Infrastructure.BasePages.ListPage
    {
        public override string PageUrl => "/Income/Index";

        public override string DeletionConfirmationModalTitle => "Delete an income";
        public override string RowClassName => "trIncome";

        private const string CostColClassName = "tdCost";

        public IncomeListPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public decimal FindCost(IWebElement firstRow)
        {
            var costValue = firstRow.FindElement(By.ClassName(CostColClassName));
            return Convert.ToDecimal(costValue.Text.Substring(1));
        }
    }
}
