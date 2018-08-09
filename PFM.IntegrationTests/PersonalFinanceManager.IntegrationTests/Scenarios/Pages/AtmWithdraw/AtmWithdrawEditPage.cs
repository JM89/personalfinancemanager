using System;
using System.Globalization;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.AtmWithdraw
{
    public class AtmWithdrawEditPage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/AtmWithdraw/Edit/{id}";

        private const string AtmWithdrawPropertyId = "Id";
        private const string CostPropertyId = "InitialAmount";

        public AtmWithdrawEditPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void EnterCost(decimal cost)
        {
            var initialAmountTxt = WebDriver.FindElement(By.Id(CostPropertyId));
            initialAmountTxt.Clear();
            initialAmountTxt.SendKeys(cost.ToString(CultureInfo.InvariantCulture));
        }

        public int FindAtmWithdrawId()
        {
            var id = WebDriver.FindElement(By.Id(AtmWithdrawPropertyId));
            return Convert.ToInt32(id.GetAttribute("value"));
        }
    }
}
